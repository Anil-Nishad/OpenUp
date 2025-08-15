using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenUp.ViewModels.Stories;
using OpenUpData;
using OpenUpData.Helpers.Enums;
using OpenUpData.Models;
using OpenUpData.Services;
using System;

namespace OpenUp.Controllers;
[Authorize]
public class StoriesController : Controller
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
        int loggedInUserId = 1;
        var imageUploadPath = await _filesService.UploadImageAsync(storyVM.Image, ImageFileType.StoryImage);

        var newStory = new Story
        {
            DateCreated = DateTime.UtcNow,
            IsDeleted = false,
            ImageUrl = imageUploadPath,
            UserId = loggedInUserId
        };

        await _storiesService.CreateStoryAsync(newStory);

        return RedirectToAction("Index", "Home");
    }
}
