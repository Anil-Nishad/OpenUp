using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenUp.ViewModels.Stories;
using OpenUpData;
using OpenUpData.Models;
using OpenUpData.Services;
using System;

namespace OpenUp.Controllers
{
    public class StoriesController : Controller
    {
        private readonly IStoriesService _storiesService;
        public StoriesController(IStoriesService storiesService)
        {
            _storiesService = storiesService;
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

            var newStory = new Story
            {
                DateCreated = DateTime.UtcNow,
                IsDeleted = false,
                UserId = loggedInUserId
            };

            await _storiesService.CreateStoryAsync(newStory, storyVM.Image);

            return RedirectToAction("Index", "Home");
        }
    }
}
