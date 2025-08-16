using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenUp.ViewModels.Settings;
using OpenUpData.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using OpenUpData.Models;

namespace OpenUp.Controllers;
[Authorize]
public class SettingsController : Controller
{
    private readonly IUsersService _usersService;
    private readonly IFilesService _filesService;
        private readonly UserManager<User> _userManager;
        public SettingsController(IUsersService usersService, 
            IFilesService filesService,
            UserManager<User> userManager) 
    {
        _usersService = usersService;
        _filesService = filesService;
            _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userDb = await _usersService.GetUser(int.Parse(loggedInUserId));

            var loggedInUser = await _userManager.GetUserAsync(User);
            return View(loggedInUser);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProfilePicture(UpdateProfilePictureVM profilePictureVM)
    {
        var loggedInUser = 1;
        var uploadedProfilePictureUrl = await _filesService.UploadImageAsync(profilePictureVM.ProfilePictureImage, OpenUpData.Helpers.Enums.ImageFileType.ProfilePicture);

        await _usersService.UpdateUserProfilePicture(loggedInUser, uploadedProfilePictureUrl);

        return RedirectToAction("Index");
    }
}
