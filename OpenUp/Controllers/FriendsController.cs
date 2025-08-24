using Microsoft.AspNetCore.Mvc;
using OpenUp.Controllers.Base;
using OpenUpData.Services;

namespace OpenUp.Controllers;

public class FriendsController : BaseController
{
    private readonly IFriendsService _friendsService;

    public FriendsController(IFriendsService friendsService)
    {
        _friendsService = friendsService;
    }

    public IActionResult Index()
    {
        return View();
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
