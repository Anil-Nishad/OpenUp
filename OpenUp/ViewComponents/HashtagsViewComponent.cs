using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenUpData;

namespace OpenUp.ViewComponents
{
    public class HashtagsViewComponent : ViewComponent
    {
        private readonly OpenUpContext _context;
        public HashtagsViewComponent(OpenUpContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var OneWeekAgo = DateTime.UtcNow.AddDays(-7);
            var hashtags = await _context.Hashtags
                .Where(h => h.DateCreated >= OneWeekAgo) 
                .OrderByDescending(h => h.Count)
                .Take(10)
                .ToListAsync();
            return View(hashtags);
        }
    }
}
