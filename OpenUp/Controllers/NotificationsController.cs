using Microsoft.AspNetCore.Mvc;
using OpenUp.Controllers.Base;
using OpenUpData.Services;

namespace OpenUp.Controllers;

public class NotificationsController : BaseController
{
    private readonly INotificationsService _notificationService;
    public NotificationsController(INotificationsService notificationService)
    {
        _notificationService = notificationService;
    }
    public IActionResult Index()
    {
        return View();
    }
    [HttpGet]
    public async Task<IActionResult> GetUnreadCount()
    {
        var userId = GetUserId();
        if (!userId.HasValue) return Json(new { success = false, message = "User not logged in." });
        //RedirectToLogin();
        var unreadCount = await _notificationService.GetUnreadNotificationsCountAsync(userId.Value);
        return Json(new { success = true,count = unreadCount });
    }
}
