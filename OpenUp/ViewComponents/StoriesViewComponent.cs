using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using OpenUpData;

namespace CircleApp.ViewComponents
{
    public class StoriesViewComponent : ViewComponent
    {
        private readonly OpenUpContext _context;
        public StoriesViewComponent(OpenUpContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var allStories = await _context.Stories
                .Where(n=> n.DateCreated >= DateTime.UtcNow.AddHours(-24)) // Get stories from the last 24 hours
                .Include(s => s.User)
                .ToListAsync();
            return View(allStories);
        }
    }
}