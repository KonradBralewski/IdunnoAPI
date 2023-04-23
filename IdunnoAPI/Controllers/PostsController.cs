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
            IEnumerable<Post> posts = await _posts.GetPostsByMatchAsync(match);

            return Ok(posts);
        }

        [Route("{postID}")]
        [HttpGet]
        public async Task<ActionResult> GetByIdAsync([FromRoute]int postID)
        {
            return Ok(await _postsService.GetPostByIdWithAuthorAsync(postID));
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody]AddPostRequestDTO post)
        {
            int newPostID = await _posts.AddPostAsync(new Post { UserId = this.GetCallerId(), PostTitle = post.PostTitle, PostDescription = post.PostDescription});
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
