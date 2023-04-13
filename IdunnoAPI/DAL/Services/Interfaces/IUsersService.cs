using IdunnoAPI.DAL.Repositories.Interfaces;
using IdunnoAPI.Models.Posts;
using IdunnoAPI.Models.Users;
using System.IdentityModel.Tokens.Jwt;

namespace IdunnoAPI.DAL.Services.Interfaces
{
    public interface IUsersService : IDisposable
    {
        IUserRepository Users { get; }
        Task<string> AuthenticateUser(User user, HttpResponse response);
        Task<KeyValuePair<UserDTO, IEnumerable<Post>>> GetUserProfileByIdAsync(int userId);
        Task<bool> RegisterUserAsync(User user);
        Task<bool> ChangeUserPasswordAsync(ChangePasswordRequest cpr);
    }
}
