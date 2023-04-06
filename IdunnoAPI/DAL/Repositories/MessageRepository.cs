using IdunnoAPI.DAL.Repositories.Interfaces;
using IdunnoAPI.Helpers;
using IdunnoAPI.Models.Messages;
using IdunnoAPI.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdunnoAPI.DAL.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IdunnoDbContext _context;
        private readonly IUserRepository _users;
        private bool disposedValue;

        public MessageRepository(IdunnoDbContext context, IUserRepository users)
        {
            _context = context;
            _users = users;
        }
        public IQueryable<Message> GetMessagesAsQueryable()
        {
            return _context.Messages.AsQueryable();
        }

        public async Task<IEnumerable<MessagesResponse>> GetMessagesByReceiverId(int receiverId)
        {
            var msgResponse = (from msg in _context.Messages.Where(m => m.ReceiverId == receiverId)
                                            join shipper in _context.Users on msg.ShipperId equals shipper.UserId
                                            select new MessagesResponse
                                            {
                                                Message = msg,
                                                ShipperName = shipper.Username
                                            }).AsQueryable();

            return await msgResponse.ToListAsync();
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

            _context.Add(msg);

            int result = await _context.SaveChangesAsync();

            if(result == 0)
            {
                throw new RequestException(StatusCodes.Status500InternalServerError, "Couldn't send your message!");
            }

            return true;
        }

        public async Task<bool> RemoveMessageAsync(int messageId)
        {
            Message toBeDeleted = new Message { MessageId = messageId };

            _context.Attach(toBeDeleted);
            _context.Remove(toBeDeleted);

            int result = await _context.SaveChangesAsync();

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
