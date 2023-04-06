using IdunnoAPI.DAL.Repositories.Interfaces;
using IdunnoAPI.Helpers;
using IdunnoAPI.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace IdunnoAPI.DAL.Repositories
{
    public class UserRepository : IUserRepository, IDisposable
    {
        private readonly IdunnoDbContext _context;
        private bool disposedValue;

        public UserRepository(IdunnoDbContext context)
        {
            _context = context;
        }
        public IQueryable<User> GetUsersAsQueryable()
        {
            return _context.Users.AsQueryable();
        }

        public async Task<User> FindUserAsync(Expression<Func<User, bool>> predicate)
        {
            User searched = await _context.Users.FirstOrDefaultAsync(predicate);

            if (searched == null) throw new RequestException(StatusCodes.Status404NotFound, "Couldn't find user.");

            return searched;
        }

        public async Task<User> FindUserAsync(int userId)
        {
            User searched = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (searched == null) throw new RequestException(StatusCodes.Status404NotFound, "Couldn't find user.");

            return searched;
        }

        public async Task<bool> AddUserAsync(User user)
        {
            User foundUser;
            try
            {
                foundUser = await FindUserAsync(u => u.Username == user.Username);
            }
            catch(RequestException re) // request is thrown when user could not be found.
            {
                _context.Users.Add(user);

                await _context.SaveChangesAsync();

                return true;
            }

            throw new RequestException(StatusCodes.Status409Conflict, "Entered login already exists.");
        }

        public async Task<bool> ChangeUserPasswordAsync(ChangePasswordRequest cpr)
        {
            User userToModify = await FindUserAsync(cpr.UserId);

            userToModify.Password = cpr.NewPassword;

            int result = await _context.SaveChangesAsync();

            if (result == 0)
            {
                throw new RequestException(StatusCodes.Status500InternalServerError, "Couldn't update user password.");
            }

            return true;
        }
        public async Task<bool> DeleteUserAsync(int userId)
        {
            throw new NotImplementedException();
        }

        
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if(_context != null)
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
