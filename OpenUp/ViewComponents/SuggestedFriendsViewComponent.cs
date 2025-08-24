using Microsoft.AspNetCore.Mvc;
using OpenUpData.Services;
using System.Security.Claims;

namespace OpenUp.ViewComponents;

public class SuggestedFriendsViewComponent : ViewComponent
{
    private readonly IFriendsService _friendsService;
    public SuggestedFriendsViewComponent(IFriendsService friendsService)
    {
        _friendsService = friendsService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var loggedInUserId = ((ClaimsPrincipal)User).FindFirstValue(ClaimTypes.NameIdentifier);
        var userId = int.Parse(loggedInUserId);

        var suggestedFriends = await _friendsService.GetSuggestedFriendsAsync(userId);
        return View(suggestedFriends);
    }
}

