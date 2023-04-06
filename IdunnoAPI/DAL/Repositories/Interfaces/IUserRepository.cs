using IdunnoAPI.Models.Users;
using System.Linq.Expressions;

namespace IdunnoAPI.DAL.Repositories.Interfaces
{
    public interface IUserRepository : IDisposable
    {
        IQueryable<User> GetUsersAsQueryable();
        Task<User> FindUserAsync(Expression<Func<User, bool>> predicate);
        Task<User> FindUserAsync(int userId);
        Task<bool> AddUserAsync(User user);
        Task<bool> DeleteUserAsync(int userId);
        Task<bool> ChangeUserPasswordAsync(ChangePasswordRequest cpr);
    }
}
