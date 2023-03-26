using IdunnoAPI.Models;

namespace IdunnoAPI.DAL.Repositories.Interfaces
{
    public interface IMessageRepository : IDisposable
    {
        Task<IEnumerable<Message>> GetMessagesByReceiverId(int receiverId);
        Task<bool> AddMessageAsync(Message msg);
        Task<bool> RemoveMessageAsync(int messageId);
    }
}
