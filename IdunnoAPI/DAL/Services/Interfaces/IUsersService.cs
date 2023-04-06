using IdunnoAPI.DAL.Repositories.Interfaces;
using IdunnoAPI.Models.Users;
using System.IdentityModel.Tokens.Jwt;

namespace IdunnoAPI.DAL.Services.Interfaces
{
    public interface IUsersService : IDisposable
    {
        IUserRepository Users { get; }
        Task<string> AuthenticateUser(User user, HttpResponse response);
        Task<UserProfile> GetUserByIdAsync(int userId);
        Task<bool> RegisterUserAsync(User user);
        Task<bool> ChangeUserPasswordAsync(ChangePasswordRequest cpr);
    }
}
