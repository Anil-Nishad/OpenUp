using Microsoft.AspNetCore.Mvc;
using OpenUp.Controllers.Base;
using OpenUp.ViewModels.Friends;
using OpenUpData.Helpers.Constants;
using OpenUpData.Models;
using OpenUpData.Services;

namespace OpenUp.Controllers;

public class FriendsController : BaseController
{
    private readonly IFriendsService _friendsService;
    private readonly INotificationsService _notificationService;

    public FriendsController(IFriendsService friendsService, INotificationsService notificationService)
    {
        _friendsService = friendsService;
        _notificationService = notificationService;
    }

    public async Task<IActionResult> Index()
    {
        var userId = GetUserId();
        if (!userId.HasValue) RedirectToLogin();

        var friendsData = new FriendshipVM()
        {
            Friends = await _friendsService.GetFriendsAsync(userId.Value),
            FriendRequestsSent = await _friendsService.GetSentFriendRequestAsync(userId.Value),
            FriendRequestsReceived = await _friendsService.GetReceivedFriendRequestAsync(userId.Value)
        };

        return View(friendsData);
    }

    [HttpPost]
    public async Task<IActionResult> SendFriendRequest(int receiverId)
    {
        var userId = GetUserId();
        var userName = GetUserFullName();
        if (!userId.HasValue) RedirectToLogin();

        await _friendsService.SendRequestAsync(userId.Value, receiverId);
        await _notificationService.AddNewNotificationAsync(receiverId, NotificationType.FriendRequest, userName, null);
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateFriendRequest(int requestId, string status)
    {
        var userId = GetUserId();
        if (!userId.HasValue) RedirectToLogin();

        var request = await _friendsService.UpdateRequestAsync(requestId, status);

        if(status == FriendshipStatus.Accepted)
        {
            var userName = GetUserFullName();
            await _notificationService.AddNewNotificationAsync(request.SenderId, NotificationType.FriendRequestApproved, userName, null);
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> RemoveFriend(int friendshipId)
    {
        await _friendsService.RemoveFriendAsync(friendshipId);
        return RedirectToAction("Index");
    }
}
