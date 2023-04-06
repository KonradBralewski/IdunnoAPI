using IdunnoAPI.DAL.Repositories.Interfaces;
using IdunnoAPI.DAL.Services.Interfaces;

namespace IdunnoAPI.DAL.Services
{
    public class MessagesService : IMessagesService
    {
        private bool _disposedValue;
        public IMessageRepository Messages { get; private set; }

        public MessagesService(IMessageRepository messages)
        {
            Messages = messages;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    if (Messages != null)
                    {
                        Messages.Dispose();
                    }
                }
                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}

