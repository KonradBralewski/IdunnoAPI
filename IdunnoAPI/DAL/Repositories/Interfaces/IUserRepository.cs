﻿using IdunnoAPI.Models.Users;
using System.Linq.Expressions;

namespace IdunnoAPI.DAL.Repositories.Interfaces
{
    public interface IUserRepository : IDisposable
    {
        IQueryable<User> GetUsersAsQueryable();
        Task<IEnumerable<UserDTO>> GetUsersByUsernameAsync(string username);
        Task<UserDTO> GetUserByIdAsync(int userId);
        Task<User> FindUserAsync(Expression<Func<User, bool>> predicate);
        Task<bool> AddUserAsync(User user);
        Task<bool> DeleteUserAsync(int userId);
        Task<bool> ChangeUserPasswordAsync(ChangePasswordRequestDTO cpr);
    }
}
