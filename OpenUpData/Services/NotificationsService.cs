using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
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

    public async Task AddNewNotificationAsync(int userId, string message, string notificationType)
    {
        var newNotification = new Notification()
        {
            UserId = userId,
            Message = message,
            Type = notificationType,
            IsRead = false,
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
}
