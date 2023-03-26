using IdunnoAPI.DAL.Repositories.Interfaces;
using IdunnoAPI.Helpers;
using IdunnoAPI.Models;
using Microsoft.EntityFrameworkCore;
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
           return await _context.Users.FirstOrDefaultAsync(predicate);
        }

        public async Task<User> FindUserAsync(int userId)
        {
            User searched = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (searched == null) throw new RequestException(StatusCodes.Status404NotFound, "Couldn't find post.");

            return searched;
        }

        public async Task<bool> AddUserAsync(User user)
        {
            if(FindUserAsync(u => u.Username == user.Username).Result != null)
                throw new RequestException(StatusCodes.Status409Conflict, "Entered login already exists.");

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateUserAsync(User user)
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
