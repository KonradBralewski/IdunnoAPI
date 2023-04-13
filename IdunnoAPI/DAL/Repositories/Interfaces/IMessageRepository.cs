using IdunnoAPI.Models.Messages;

namespace IdunnoAPI.DAL.Repositories.Interfaces
{
    public interface IMessageRepository : IDisposable
    {
        IQueryable<Message> GetMessagesAsQueryable();
        Task<IEnumerable<MessageDTO>> GetMessagesByReceiverId(int receiverId);
        Task<IEnumerable<MessageDTO>> BuildConversationAsync(int receiverId, int shipperId);
        Task<bool> AddMessageAsync(Message msg);
        Task<bool> RemoveMessageAsync(int messageId);
        
    }
}
