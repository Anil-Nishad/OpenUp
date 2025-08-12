using CircleApp.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenUp.ViewModels.Home;
using OpenUpData;
using OpenUpData.Helpers;
using OpenUpData.Models;
using System.Diagnostics;

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
            int loggedInUserId = 1;

            var allPosts = await _context.Posts
                .Where(n => (!n.IsPrivate || n.UserId == loggedInUserId) && n.Reports.Count < 5 && !n.IsDeleted)
                        .Include(n => n.User)
                        .Include(n => n.Likes)
                        .Include(n => n.Favorites)
                        .Include(n => n.Comments).ThenInclude(n => n.User)
                .Include(n => n.Reports)
                        .OrderByDescending(n => n.DateCreated)
                        .ToListAsync();
            return View(allPosts);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(PostVM post)
        {
            //Get the logged in user
            int loggedInUser = 1;

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
                    string rootFolderPathImages = Path.Combine(rootFolderPath, "images/posts");
                    Directory.CreateDirectory(rootFolderPathImages);

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(post.Image.FileName);
                    string filePath = Path.Combine(rootFolderPathImages, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                        await post.Image.CopyToAsync(stream);

                    //Set the URL to the newPost object
                    newPost.ImageUrl = "/images/posts/" + fileName;
                }
            }

            //Add the post to the database
            await _context.Posts.AddAsync(newPost);
            await _context.SaveChangesAsync();

            var postHashtags = HashtagHelper.GetHashtags(post.Content);

            // Save hashtags to the database
            foreach (var hashtag in postHashtags)
            {
                var existingHashtag = await _context.Hashtags
                    .FirstOrDefaultAsync(h => h.Name == hashtag);

                if (existingHashtag != null)
                {
                    existingHashtag.Count++;
                    existingHashtag.DateUpdated = DateTime.UtcNow;
                    _context.Hashtags.Update(existingHashtag);
                }
                else
                {
                    var newHashtag = new Hashtag
                    {
                        Name = hashtag,
                        Count = 1,
                        DateCreated = DateTime.UtcNow,
                        DateUpdated = DateTime.UtcNow
                    };
                    await _context.Hashtags.AddAsync(newHashtag);
                }
            }

            await _context.SaveChangesAsync();

            //Redirect to the Index Page
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> TogglePostLike(PostLikeVM postLikeVM)
        {
            int loggedInUserId = 1;

            //check if user has already liked the post
            var like = await _context.Likes
                .Where(l => l.PostId == postLikeVM.PostId && l.UserId == loggedInUserId)
                .FirstOrDefaultAsync();

            if (like != null)
            {
                _context.Likes.Remove(like);
                await _context.SaveChangesAsync();
            }
            else
            {
                var newLike = new Like()
                {
                    PostId = postLikeVM.PostId,
                    UserId = loggedInUserId
                };
                await _context.Likes.AddAsync(newLike);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> TogglePostFavorite(PostFavoriteVM postFavoriteVM)
        {
            int loggedInUserId = 1;

            //check if user has already favorited the post
            var favorite = await _context.Favorites
                .Where(l => l.PostId == postFavoriteVM.PostId && l.UserId == loggedInUserId)
                .FirstOrDefaultAsync();

            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                await _context.SaveChangesAsync();
            }
            else
            {
                var newFavorite = new Favorite()
                {
                    PostId = postFavoriteVM.PostId,
                    UserId = loggedInUserId
                };
                await _context.Favorites.AddAsync(newFavorite);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> TogglePostVisibility(PostVisibilityVM postVisibilityVM)
        {
            int loggedInUserId = 1;

            //get post by id and loggedin user id
            var post = await _context.Posts
                .FirstOrDefaultAsync(l => l.Id == postVisibilityVM.PostId && l.UserId == loggedInUserId);

            if (post != null)
            {
                post.IsPrivate = !post.IsPrivate;
                _context.Posts.Update(post);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddPostComment(PostCommentVM postCommentVM)
        {
            int loggedInUserId = 1;

            //Creat a post object
            var newComment = new Comment()
            {
                UserId = loggedInUserId,
                PostId = postCommentVM.PostId,
                Content = postCommentVM.Content,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow
            };
            await _context.Comments.AddAsync(newComment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddPostReport(PostReportVM postReportVM)
        {
            int loggedInUserId = 1;

            //Creat a post object
            var newReport = new Report()
            {
                UserId = loggedInUserId,
                PostId = postReportVM.PostId,
                DateCreated = DateTime.UtcNow,
            };
            await _context.Reports.AddAsync(newReport);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemovePostComment(RemoveCommentVM removeCommentVM)
        {
            var commentDb = await _context.Comments.FirstOrDefaultAsync(c => c.Id == removeCommentVM.CommentId);

            if (commentDb != null)
            {
                _context.Comments.Remove(commentDb);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> PostRemove(PostRemoveVM postRemoveVM)
        {
            var postDb = await _context.Posts.FirstOrDefaultAsync(c => c.Id == postRemoveVM.PostId);

            if(postDb != null)
            {
                postDb.IsDeleted = true;
                _context.Posts.Update(postDb);
                await _context.SaveChangesAsync();

                //Update Hashtags count
                var postHashtags = HashtagHelper.GetHashtags(postDb.Content);
                foreach (var hashtag in postHashtags)
                {
                    var existingHashtag = await _context.Hashtags
                        .FirstOrDefaultAsync(h => h.Name == hashtag);
                    if (existingHashtag != null)
                    {
                        existingHashtag.Count--;
                        if (existingHashtag.Count <= 0)
                        {
                            _context.Hashtags.Remove(existingHashtag);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            existingHashtag.DateUpdated = DateTime.UtcNow;
                            _context.Hashtags.Update(existingHashtag);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }
           
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }
        
    }
}
