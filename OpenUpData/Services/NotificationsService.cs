using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using OpenUpData.Helpers.Constants;
using OpenUpData.Hubs;
using OpenUpData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenUpData.Services;

public class NotificationsService : INotificationsService
{
    private readonly OpenUpContext _context;
    private readonly IHubContext<NotificationHub> _hubContext;
    public NotificationsService(OpenUpContext context, IHubContext<NotificationHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }

    public async Task AddNewNotificationAsync(int userId, string notificationType, string userFullName, int? postId)
    {
        var newNotification = new Notification()
        {
            UserId = userId,
            Message = GetPostMessage(notificationType, userFullName),
            Type = notificationType,
            IsRead = false,
            PostId = postId,
            DateCreated = DateTime.UtcNow,
            DateUpdated = DateTime.UtcNow
        };

        await _context.Notifications.AddAsync(newNotification);
        await _context.SaveChangesAsync();

        var notificationNumber = await GetUnreadNotificationsCountAsync(userId);

        await _hubContext.Clients.User(userId.ToString())
            .SendAsync("ReceiveNotification", notificationNumber);
    }

    public async Task<int> GetUnreadNotificationsCountAsync(int userId)
    {
        return await _context.Notifications.CountAsync(n => n.UserId == userId && !n.IsRead);
    }

    public async Task<List<Notification>> GetUserNotificationsAsync(int userId)
    {
        {
            var allNotifications = await _context.Notifications.Where(n => n.UserId == userId)
                .OrderBy(n => n.IsRead)
                .ThenByDescending(n => n.DateCreated)
                .ToListAsync();

            return allNotifications;
        }
    }

    private string GetPostMessage(string notificationType, string userFullName)
    {
        return notificationType switch
        {
            NotificationType.Like => $"{userFullName} liked your post.",
            NotificationType.Comment => $"{userFullName} commented on your post.",
            NotificationType.Favorite => $"{userFullName} favorited your post.",
            NotificationType.FriendRequest => $"{userFullName} sent you a friend request.",
            NotificationType.FriendRequestApproved => $"{userFullName} accepted your friend request.",
            _ => "You have a new notification."
        };
    }
}
