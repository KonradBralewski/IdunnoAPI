using IdunnoAPI.Models.Messages;
using IdunnoAPI.Models.Users;
using System.Linq.Expressions;

namespace IdunnoAPI.DAL.Repositories.Interfaces
{
    public interface IMessageRepository : IDisposable
    {
        IQueryable<Message> GetMessagesAsQueryable();
        Task<IEnumerable<MessageDTO>> GetMessagesByReceiverId(int receiverId);
        Task<IEnumerable<MessageDTO>> BuildConversationAsync(int receiverId, int shipperId);
        Task<Message> FindMessageAsync(Expression<Func<Message, bool>> predicate);
        Task<bool> AddMessageAsync(Message msg);
        Task<bool> RemoveMessageAsync(int messageId);
        
    }
}
