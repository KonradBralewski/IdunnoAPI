using IdunnoAPI.DAL.Repositories.Interfaces;
using IdunnoAPI.DAL.Services;
using IdunnoAPI.DAL.Services.Interfaces;
using IdunnoAPI.Extensions;
using IdunnoAPI.Helpers;
using IdunnoAPI.Models.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdunnoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostsController : BaseIdunnoController
    {
        private readonly IPostsService _postsService;
        private readonly IPostRepository _posts;
        public PostsController(IPostsService postsService, IPostRepository postsRepo)
        {
            _postsService = postsService;
            _posts = postsRepo;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            IEnumerable<Post> posts = _posts.GetPostsAsQueryable().ToList();

            return Ok(posts);
        }

        [Route("ByMatch")]
        [HttpGet]
        public async Task<ActionResult> GetPostsByMatchAsync([FromQuery]string match)
        {
            IEnumerable<Post> posts = await _postsService.GetPostsByMatch(match);

            return Ok(posts);
        }

        [Route("{postID}")]
        [HttpGet]
        public async Task<ActionResult> GetByIdAsync([FromRoute]int postID)
        {
            return Ok(await _postsService.GetPostByIdWithAuthor(postID));
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody]Post post) // this.GetCallerId() because even if userId will be passed in request we will ignore it, we don't want it from clientside.
        {
            post.UserId = this.GetCallerId();
            int newPostID = await _posts.AddPostAsync(post);
            return Created($"api/Posts/{newPostID}", post);
        }

        [Route("{postId}")]
        [HttpDelete]
        public async Task<ActionResult> DeleteAsync([FromRoute]int postId)
        {
            await _posts.DeletePostAsync(postId);

            return Ok(); // to check
        }

        [HttpPatch]
        public async Task<ActionResult> UpdateAsync([FromBody] Post post)
        {
            await _posts.UpdatePostAsync(post);

            return NoContent(); // to check
        }
    }
}
