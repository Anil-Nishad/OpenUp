using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenUp.ViewModels.Home;
using OpenUpData;
using OpenUpData.Models;

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
                        .Include(n => n.User)
                        .OrderByDescending(n => n.DateCreated)
                        .ToListAsync();
            return View(allPosts);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(PostVM post)
        {
            //Get the Logged in User
            int loggedInUser = 1;

            //Create a new Post
            var newPost = new Post
            {
                Content = post.Content,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                UserId = loggedInUser
            };
            await _context.Posts.AddAsync(newPost);
            await _context.SaveChangesAsync();

            //Redirect to the Index Page
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }
        
    }
}
