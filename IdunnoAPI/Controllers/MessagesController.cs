using IdunnoAPI.DAL.Repositories.Interfaces;
using IdunnoAPI.Extensions;
using IdunnoAPI.Models;
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
        public MessagesController(IMessageRepository messageRepository)
        {
            _messages = messageRepository;
        }

        [HttpGet]
        public async Task<ActionResult> GetMessagesAsync() // this default GET Method (.../api/Messages) will return calling user messages
        {
            IEnumerable<Message> messages = await _messages.GetMessagesByReceiverId(this.GetCallerId());

            return Ok(messages);
        }

        [HttpPost]
        public async Task<ActionResult> SendMessageAsync([FromBody]Message msg)
        {
            msg.ShipperId = this.GetCallerId();
            await _messages.AddMessageAsync(msg);

            return Ok(); // TODO: check if this is acceptable in REST convention
        }

        [Route("{messageId}")]
        [HttpDelete]
        public async Task<ActionResult> DeleteMessageAsync([FromRoute]int messageId)
        {
            await _messages.RemoveMessageAsync(messageId);

            return Ok();
        }


    }
}
