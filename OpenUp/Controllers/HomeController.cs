using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenUpData;

namespace OpenUp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly OpenUpContext _context;

        public HomeController(ILogger<HomeController> logger, OpenUpContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var allPosts = await _context.Posts
                        .Include(n=>n.User)
                        .ToListAsync();
            return View(allPosts);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        
    }
}
