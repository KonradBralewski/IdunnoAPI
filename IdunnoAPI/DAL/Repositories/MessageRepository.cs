using IdunnoAPI.DAL.Repositories.Interfaces;
using IdunnoAPI.Helpers;
using IdunnoAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace IdunnoAPI.DAL.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IdunnoDbContext _dbContext;
        private readonly IUserRepository _users;
        private bool disposedValue;

        public MessageRepository(IdunnoDbContext dbContext, IUserRepository users)
        {
            _dbContext = dbContext;
            _users = users;
        }
        public async Task<IEnumerable<Message>> GetMessagesByReceiverId(int receiverId)
        {
            IQueryable<Message> messages;

            messages = _dbContext.Messages.Where(msg => msg.ReceiverId == receiverId);

            return await messages.ToListAsync();
        }
        public async Task<bool> AddMessageAsync(Message msg)
        {
            User shipper = new User { UserId = msg.ShipperId };
            User receiver = new User { UserId = msg.ReceiverId };

            if(_users.FindUserAsync(u=>u.UserId == shipper.UserId).Result == null 
                || _users.FindUserAsync(u => u.UserId == receiver.UserId).Result == null)
            {
                throw new RequestException(StatusCodes.Status500InternalServerError, "Couldn't send your message!");
            }

            _dbContext.Add(msg);

            int result = await _dbContext.SaveChangesAsync();

            if(result == 0)
            {
                throw new RequestException(StatusCodes.Status500InternalServerError, "Couldn't send your message!");
            }

            return true;
        }

        public async Task<bool> RemoveMessageAsync(int messageId)
        {
            Message toBeDeleted = new Message { MessageId = messageId };

            _dbContext.Attach(toBeDeleted);
            _dbContext.Remove(toBeDeleted);

            int result = await _dbContext.SaveChangesAsync();

            if (result == 0)
            {
                throw new RequestException(StatusCodes.Status500InternalServerError, "Couldn't delete your message!");
            }

            return true;

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                disposedValue = true;
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
