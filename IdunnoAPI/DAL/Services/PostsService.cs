using IdunnoAPI.DAL.Repositories;
using IdunnoAPI.DAL.Repositories.Interfaces;
using IdunnoAPI.DAL.Services.Interfaces;
using IdunnoAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Web;

namespace IdunnoAPI.DAL.Services
{
    /// <summary>   Service layer. Return types should be taken with a grain of salt. Error checking is already handled by repository layer by throwing exceptions.
    public class PostsService : IPostsService, IDisposable
    {
        private bool disposedValue;

        public IPostRepository Posts { get; private set; }
        private readonly IUserRepository _users;

        public PostsService(IPostRepository posts, IUserRepository users)
        {
            Posts = posts;
            _users = users;
        }

        public async Task<IEnumerable<Post>> GetPostsByMatch(string match)
        {
            IQueryable<Post> posts = Posts.GetPostsAsQueryable();

            return await posts.Where(p => p.PostTitle.Contains(match) || p.PostDescription.Contains(match)).ToListAsync();
        }

        public async Task<KeyValuePair<Post, string>> GetPostByIdWithAuthor(int postId)
        {
            Post post = await Posts.FindPostAsync(postId);

            User author = await _users.FindUserAsync(post.UserId);

            return new KeyValuePair<Post, string>(post, author.Username);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (Posts != null)
                    {
                        Posts.Dispose();
                    }
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
