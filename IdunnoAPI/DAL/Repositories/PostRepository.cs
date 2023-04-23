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

        public async Task<IEnumerable<Post>> GetPostsByMatchAsync(string match)
        {
            if(match == String.Empty)
            {
                throw new RequestException(StatusCodes.Status400BadRequest, "Search query is empty. Bad Request");
            }

            IEnumerable<Post> posts = await GetPostsAsQueryable().Where(p => p.PostTitle.Contains(match) || p.PostDescription.Contains(match)).ToListAsync();

            if (posts == null)
            {
                throw new RequestException(StatusCodes.Status500InternalServerError, "We couldn't process posts search. Server error.");
            }

            if (posts.Count() == 0)
            {
                throw new RequestException(statusCode: StatusCodes.Status404NotFound, "Couldn't find any post.");
            }

               
            return posts;
        }

        public async Task<Post> FindPostAsync(Expression<Func<Post, bool>> predicate)
        {
            Post searched = await _context.Posts.FirstOrDefaultAsync(predicate);

            if (searched == null)
            {
                throw new RequestException(StatusCodes.Status404NotFound, "Couldn't find post.");
            }

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

        public async Task<bool> DeletePostAsync(int postId)
        {
            Post postToDelete = await FindPostAsync(p => p.PostId == postId);

            _context.Posts.Attach(postToDelete);
            _context.Posts.Remove(postToDelete);

            int result = await _context.SaveChangesAsync();

            if (result == 0)
            {
                throw new RequestException(StatusCodes.Status500InternalServerError, "Couldn't delete post. Server error.");
            }

            return true;
        }


        public async Task<bool> UpdatePostAsync(Post post)
        {
            Post postToModify = await FindPostAsync(p=> p.PostId == post.PostId);

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
