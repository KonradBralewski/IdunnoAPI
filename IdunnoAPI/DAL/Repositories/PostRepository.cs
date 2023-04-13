using IdunnoAPI.DAL.Repositories.Interfaces;
using IdunnoAPI.Helpers;
using IdunnoAPI.Models.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Linq.Expressions;

namespace IdunnoAPI.DAL.Repositories
{
    public class PostRepository : IPostRepository, IDisposable
    {
        private readonly IdunnoDbContext _context;
        private bool disposedValue;

        public PostRepository(IdunnoDbContext context)
        {
            _context = context;
        }
        public IQueryable<Post> GetPostsAsQueryable()
        {
            return _context.Posts.AsQueryable();
        }

        /// <summary>
        ///  Null checking only in postId overload as it will probably be thrown straight from controller.
        /// </summary>
        /// 
        public async Task<IEnumerable<Post>> GetPostsByMatchAsync(string match)
        {
            IEnumerable<Post> posts = await GetPostsAsQueryable().Where(p => p.PostTitle.Contains(match) || p.PostDescription.Contains(match)).ToListAsync();

            if (posts == null) throw new RequestException(StatusCodes.Status404NotFound, "Couldn't find any posts.");

            return posts;

        }
        public async Task<Post> FindPostAsync(Expression<Func<Post, bool>> predicate)
        {
            return await _context.Posts.FirstOrDefaultAsync(predicate);
        }

        public async Task<Post> FindPostAsync(int postId)
        {
            Post searched = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == postId);

            if (searched == null) throw new RequestException(StatusCodes.Status404NotFound, "Couldn't find post.");

            return searched;
        }

        public async Task<int> AddPostAsync(Post post)
        {
            _context.Posts.Add(post);

            int result = await _context.SaveChangesAsync();

            if (result == 0)
            {
                throw new RequestException(StatusCodes.Status500InternalServerError, "Couldn't add post. Server error.");
            }

            return post.PostId;
        }

        public async Task<bool> DeletePostAsync(int postID)
        {
            Post post = new Post { PostId = postID };

            _context.Posts.Attach(post);
            _context.Posts.Remove(post);

            int result = await _context.SaveChangesAsync();

            if (result == 0)
            {
                throw new RequestException(StatusCodes.Status500InternalServerError, "Couldn't delete post. Server error.");
            }

            return true;
        }


        public async Task<bool> UpdatePostAsync(Post post)
        {
            Post postToModify = await FindPostAsync(post.PostId);

            postToModify.PostTitle = post.PostTitle;
            postToModify.PostDescription = post.PostDescription;

            int result = await _context.SaveChangesAsync();

            if (result == 0)
            {
                throw new RequestException(StatusCodes.Status500InternalServerError, "Couldn't update post. Server error.");
            }

            return true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_context != null)
                    {
                        _context.Dispose();
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
