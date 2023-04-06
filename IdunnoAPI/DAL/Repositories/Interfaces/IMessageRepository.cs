using IdunnoAPI.Models.Messages;

namespace IdunnoAPI.DAL.Repositories.Interfaces
{
    public interface IMessageRepository : IDisposable
    {
        IQueryable<Message> GetMessagesAsQueryable();
        Task<IEnumerable<MessagesResponse>> GetMessagesByReceiverId(int receiverId);
        Task<bool> AddMessageAsync(Message msg);
        Task<bool> RemoveMessageAsync(int messageId);
        
    }
}
