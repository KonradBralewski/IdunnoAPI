using System.ComponentModel.DataAnnotations;

namespace IdunnoAPI.Models.Messages
{
    public class SendMessageRequest
    {
        [Required] public int ReceiverId { get; set; }
        [Required] public string Msg { get; set; }
    }
}
