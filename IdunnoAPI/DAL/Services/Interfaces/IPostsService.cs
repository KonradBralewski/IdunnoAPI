using IdunnoAPI.DAL.Repositories.Interfaces;
using IdunnoAPI.Models.Posts;

namespace IdunnoAPI.DAL.Services.Interfaces
{
    public interface IPostsService : IDisposable
    {
        IPostRepository Posts { get; }
        Task<KeyValuePair<Post, string>> GetPostByIdWithAuthorAsync(int postId);
    }
}
