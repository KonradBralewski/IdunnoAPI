using IdunnoAPI.Models.Posts;
using System.Linq.Expressions;

namespace IdunnoAPI.DAL.Repositories.Interfaces
{
    public interface IPostRepository : IDisposable
    {
        IQueryable<Post> GetPostsAsQueryable();
        Task<IEnumerable<Post>> GetPostsByMatchAsync(string match);
        Task<Post> FindPostAsync(Expression<Func<Post, bool>> predicate);
        Task<int> AddPostAsync(Post post);
        Task<bool> UpdatePostAsync(Post post);
        Task<bool> DeletePostAsync(int postId);
    }
}
