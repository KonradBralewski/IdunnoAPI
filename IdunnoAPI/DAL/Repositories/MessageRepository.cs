﻿using IdunnoAPI.DAL.Repositories.Interfaces;
using IdunnoAPI.Helpers;
using IdunnoAPI.Models.Messages;
using IdunnoAPI.Models.Posts;
using IdunnoAPI.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

        public async Task<IEnumerable<MessageDTO>> GetMessagesByReceiverId(int receiverId)
        {
            IQueryable<MessageDTO> messages = (from msg in _context.Messages.Where(m => m.ReceiverId == receiverId || m.isGlobalMessage)
                                            join shipper in _context.Users on msg.ShipperId equals shipper.UserId
                                            select new MessageDTO
                                            {
                                                Message = msg,
                                                ShipperName = shipper.Username
                                            }).OrderBy(m => m.ShipperName)
                                            .AsQueryable();

            if (messages == null) throw new RequestException(StatusCodes.Status404NotFound, "Couldn't find user messages.");

            return await messages.ToListAsync();
        }

        public async Task<IEnumerable<MessageDTO>> BuildConversationAsync(int receiverId, int shipperId)
        {
            await _users.FindUserAsync(u=> u.UserId == shipperId); // Throws exception if user doesn't exist.

            IQueryable<MessageDTO> messages = (from msg in _context.Messages.Where(m => (m.ReceiverId == receiverId && m.ShipperId == shipperId) ||
                                               (m.ReceiverId == shipperId && m.ShipperId == receiverId))
                                               join shipper in _context.Users on msg.ShipperId equals shipper.UserId
                                               select new MessageDTO
                                               {
                                                   Message = msg,
                                                   ShipperName = shipper.Username
                                               }).AsQueryable();

            if (messages == null) throw new RequestException(StatusCodes.Status404NotFound, "Couldn't find user conversation.");

            return await messages.ToListAsync();
        }

        public async Task<Message> FindMessageAsync(Expression<Func<Message, bool>> predicate)
        {
            Message searched = await _context.Messages.FirstOrDefaultAsync(predicate);

            if (searched == null) throw new RequestException(StatusCodes.Status404NotFound, "Couldn't find message.");

            return searched;
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
