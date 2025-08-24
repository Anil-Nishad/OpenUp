using Microsoft.AspNetCore.Mvc;
using OpenUp.Controllers.Base;
using OpenUp.ViewModels.Friends;
using OpenUpData.Models;
using OpenUpData.Services;

namespace OpenUp.Controllers;

public class FriendsController : BaseController
{
    private readonly IFriendsService _friendsService;

    public FriendsController(IFriendsService friendsService)
    {
        _friendsService = friendsService;
    }

    public async Task<IActionResult> Index()
    {
        var userId = GetUserId();
        if (!userId.HasValue) RedirectToLogin();

        var friendsData = new FriendshipVM()
        {
            FriendRequestSent = await _friendsService.GetSentFriendRequestAsync(userId.Value)
        };

        return View(friendsData);
    }

    [HttpPost]
    public async Task<IActionResult> SendFriendRequest(int receiverId)
    {
        var userId = GetUserId();
        if (!userId.HasValue) RedirectToLogin();

        await _friendsService.SendRequestAsync(userId.Value, receiverId);
        return RedirectToAction("Index", "Home");
    }
}
