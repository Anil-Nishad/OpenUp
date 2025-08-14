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
        private readonly IHashtagsService _hashtagsService;
        private readonly IPostsService _postsService;

        public HomeController(ILogger<HomeController> logger, IHashtagsService hashtagsService, IPostsService postsService)
        {
            _logger = logger;
            _hashtagsService = hashtagsService;
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
            await _hashtagsService.ProcessHashtagsForNewPostAsync(post.Content);
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
            var postRemoved = await _postsService.RemovePostAsync(postRemoveVM.PostId);
            await _hashtagsService.ProcessHashtagsForRemovedPostAsync(postRemoved.Content);
            return RedirectToAction("Index");
        }

    }
}
