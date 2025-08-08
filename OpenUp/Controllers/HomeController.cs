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
            // Check if content is empty or null
            if (string.IsNullOrWhiteSpace(post?.Content))
            {
                ModelState.AddModelError("Content", "Content is required.");
                // Optionally, you can return the same view with the model to show validation errors
                // return View("Index", await _context.Posts.Include(n => n.User).OrderByDescending(n => n.DateCreated).ToListAsync());
                return BadRequest("Content is required.");
            }

            //Get the Logged in User
            int loggedInUser = 2;

            //Create a new Post
            var newPost = new Post
            {
                Content = post.Content,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                UserId = loggedInUser
            };

            //Check if an image is uploaded
            if (post.Image != null && post.Image.Length > 0)
            {
                string rootFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                if (post.Image.ContentType.Contains("image"))
                {
                    string rootFolderPathImages = Path.Combine(rootFolderPath, "images/uploaded");
                    Directory.CreateDirectory(rootFolderPathImages);

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(post.Image.FileName);
                    string filePath = Path.Combine(rootFolderPathImages, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                        await post.Image.CopyToAsync(stream);

                    //Set the URL to the newPost object
                    newPost.ImageUrl = "/images/uploaded/" + fileName;
                }
            }

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
