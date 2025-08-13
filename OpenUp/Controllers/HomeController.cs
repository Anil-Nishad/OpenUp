using CircleApp.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenUp.ViewModels.Home;
using OpenUpData;
using OpenUpData.Helpers;
using OpenUpData.Models;
using OpenUpData.Services;
using System.Diagnostics;

namespace OpenUp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly OpenUpContext _context;
        private readonly IPostsService _postsService;

        public HomeController(ILogger<HomeController> logger, OpenUpContext context, IPostsService postsService)
        {
            _logger = logger;
            _context = context;
            _postsService = postsService;
        }

        public async Task<IActionResult> Index()
        {
            int loggedInUserId = 1;
            var allPosts = await _postsService.GetAllPostsAsync(loggedInUserId);
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

            await _postsService.CreatePostAsync(newPost, post.Image);

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
            await _postsService.TogglePostLikeAsync(postLikeVM.PostId, loggedInUserId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> TogglePostFavorite(PostFavoriteVM postFavoriteVM)
        {
            int loggedInUserId = 1;
            await _postsService.TogglePostFavoriteAsync(postFavoriteVM.PostId, loggedInUserId);
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> TogglePostVisibility(PostVisibilityVM postVisibilityVM)
        {
            int loggedInUserId = 1;
            await _postsService.TogglePostVisibilityAsync(postVisibilityVM.PostId, loggedInUserId);
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
            await _postsService.AddPostCommentAsync(newComment);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddPostReport(PostReportVM postReportVM)
        {
            int loggedInUserId = 1;
            await _postsService.ReportPostAsync(postReportVM.PostId, loggedInUserId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemovePostComment(RemoveCommentVM removeCommentVM)
        {
            await _postsService.RemovePostCommentAsync(removeCommentVM.CommentId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> PostRemove(PostRemoveVM postRemoveVM)
        {
            await _postsService.RemovePostAsync(postRemoveVM.PostId);
            ////Update Hashtags count
            //var postHashtags = HashtagHelper.GetHashtags(postDb.Content);
            //foreach (var hashtag in postHashtags)
            //{
            //    var existingHashtag = await _context.Hashtags
            //        .FirstOrDefaultAsync(h => h.Name == hashtag);
            //    if (existingHashtag != null)
            //    {
            //        existingHashtag.Count--;
            //        if (existingHashtag.Count <= 0)
            //        {
            //            _context.Hashtags.Remove(existingHashtag);
            //            await _context.SaveChangesAsync();
            //        }
            //        else
            //        {
            //            existingHashtag.DateUpdated = DateTime.UtcNow;
            //            _context.Hashtags.Update(existingHashtag);
            //            await _context.SaveChangesAsync();
            //        }
            //    }
            //}
            return RedirectToAction("Index");
        }

    }
}
