using IdunnoAPI.Models;
using System.Linq.Expressions;

namespace IdunnoAPI.DAL.Repositories.Interfaces
{
    public interface IPostRepository : IDisposable
    {
        IQueryable<Post> GetPostsAsQueryable();
        Task<Post> FindPostAsync(Expression<Func<Post, bool>> predicate);
        Task<Post> FindPostAsync(int postId);
        Task<int> AddPostAsync(Post post);
        Task<bool> UpdatePostAsync(Post post);
        Task<bool> DeletePostAsync(int postId);
    }
}
