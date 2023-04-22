using System.ComponentModel.DataAnnotations;

namespace IdunnoAPI.Models.Messages
{
    public class SendMessageRequestDTO
    {
        [Required] public int ReceiverId { get; set; }
        [Required] public string Msg { get; set; }
    }
}
