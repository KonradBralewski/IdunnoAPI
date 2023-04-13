using IdunnoAPI.DAL.Repositories.Interfaces;
using IdunnoAPI.DAL.Services.Interfaces;
using IdunnoAPI.Extensions;
using IdunnoAPI.Models.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdunnoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessagesController : BaseIdunnoController
    {
        private readonly IMessageRepository _messages;
        private readonly IMessagesService _messagesService;
        public MessagesController(IMessageRepository messageRepository, IMessagesService messagesService)
        {
            _messages = messageRepository;
            _messagesService = messagesService;
        }

        [HttpGet]
        [Route("CurrentUser")]
        public async Task<ActionResult> GetCurrentUserMessagesAsync()
        {
            IEnumerable<MessageDTO> messages = await _messages.GetMessagesByReceiverId(this.GetCallerId());
            
            return Ok(messages);
        }

        [HttpPost]
        public async Task<ActionResult> SendMessageAsync([FromBody]SendMessageRequest smr)
        {
            await _messages.AddMessageAsync(new Message { Msg = smr.Msg, ShipperId = this.GetCallerId(), ReceiverId = smr.ReceiverId});

            return Ok("Message has been sent.");
        }

        [HttpDelete]
        [Route("{messageId}")]
        public async Task<ActionResult> DeleteMessageAsync([FromRoute]int messageId)
        {
            await _messages.RemoveMessageAsync(messageId);

            return Ok();
        }


    }
}
