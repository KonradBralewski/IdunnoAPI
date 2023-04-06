using IdunnoAPI.DAL.Repositories.Interfaces;

namespace IdunnoAPI.DAL.Services.Interfaces
{
    public interface IMessagesService : IDisposable
    {
        IMessageRepository Messages { get; }
    }
}
