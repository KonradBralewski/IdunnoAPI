using IdunnoAPI.DAL.Repositories;
using IdunnoAPI.DAL.Repositories.Interfaces;
using IdunnoAPI.DAL.Services;
using IdunnoAPI.DAL.Services.Interfaces;
using IdunnoAPI.Extensions;
using IdunnoAPI.Models.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IdunnoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ConversationController : BaseIdunnoController
    {
        private readonly IMessageRepository _messages;
        private readonly IMessagesService _messagesService;
        public ConversationController(IMessageRepository messageRepository, IMessagesService messagesService)
        {
            _messages = messageRepository;
            _messagesService = messagesService;
        }

        [HttpGet]
        [Route("{shipperId}")]
        public async Task<ActionResult> BuildConversationAsync([FromRoute]int shipperId)
        {
            IEnumerable<MessageDTO> conversation = await _messages.BuildConversationAsync(this.GetCallerId(), shipperId);

            return Ok(conversation);
        }
    }
}
