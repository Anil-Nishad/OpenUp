using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenUp.Controllers.Base;
using OpenUp.ViewModels.Stories;
using OpenUpData.Helpers.Constants;
using OpenUpData.Helpers.Enums;
using OpenUpData.Models;
using OpenUpData.Services;

namespace OpenUp.Controllers;
[Authorize(Roles = AppRoles.User)]
public class StoriesController : BaseController
{
    private readonly IStoriesService _storiesService;
    private readonly IFilesService _filesService;
    public StoriesController(IStoriesService storiesService, IFilesService filesService)
    {
        _storiesService = storiesService;
        _filesService = filesService;
    }

    //public async Task<IActionResult> Index()
    //{
    //    var allStories = await _context.Stories.Include(s => s.User).ToListAsync();
    //    return View(allStories);
    //}

    [HttpPost]
    public async Task<IActionResult> CreateStory(StoryVM storyVM)
    {
            var loggedInUserId = GetUserId();
            if (loggedInUserId == null) return RedirectToLogin();

        var imageUploadPath = await _filesService.UploadImageAsync(storyVM.Image, ImageFileType.StoryImage);

        var newStory = new Story
        {
            DateCreated = DateTime.UtcNow,
            IsDeleted = false,
            ImageUrl = imageUploadPath,
            UserId = loggedInUserId.Value
        };

        await _storiesService.CreateStoryAsync(newStory);

        return RedirectToAction("Index", "Home");
    }
}
