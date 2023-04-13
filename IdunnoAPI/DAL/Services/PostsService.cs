using IdunnoAPI.DAL.Repositories;
using IdunnoAPI.DAL.Repositories.Interfaces;
using IdunnoAPI.DAL.Services.Interfaces;
using IdunnoAPI.Helpers;
using IdunnoAPI.Models.Posts;
using IdunnoAPI.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Web;

namespace IdunnoAPI.DAL.Services
{
    /// <summary>   Service layer. Return types should be taken with a grain of salt. Error checking is already handled by repository layer by throwing exceptions.
    public class PostsService : IPostsService, IDisposable
    {
        private bool _disposedValue;

        public IPostRepository Posts { get; private set; }
        private readonly IUserRepository _users;

        public PostsService(IPostRepository posts, IUserRepository users)
        {
            Posts = posts;
            _users = users;
        }

        public async Task<KeyValuePair<Post, string>> GetPostByIdWithAuthorAsync(int postId)
        {
            Post post = await Posts.FindPostAsync(postId);

            string author;
            try
            {
                User authorUser = await _users.FindUserAsync(post.UserId);
                author = authorUser.Username;
            }
            catch (RequestException notFoundException)
            {
                author = "Account deleted";
            }

            return new KeyValuePair<Post, string>(post, author);

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    if (Posts != null)
                    {
                        Posts.Dispose();
                    }

                    if(_users != null)
                    {
                        _users.Dispose();
                    }
                    
                }
                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
