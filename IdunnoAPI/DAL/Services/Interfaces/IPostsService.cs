using IdunnoAPI.Models;
using IdunnoAPI.DAL.Repositories.Interfaces;

namespace IdunnoAPI.DAL.Services.Interfaces
{
    public interface IPostsService : IDisposable
    {
        IPostRepository Posts { get; }
        Task<IEnumerable<Post>> GetPostsByMatch(string match);
        Task<KeyValuePair<Post, string>> GetPostByIdWithAuthor(int postId);
    }
}
