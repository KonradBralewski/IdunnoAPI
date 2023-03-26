using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace IdunnoAPI.Models
{
    [Table("Messages")]
    public class Message
    {
        [Key][Required] public int MessageId { get; set; }
        [Required] public int ShipperId { get; set; }
        [Required] public int ReceiverId { get; set; }
        [Required] public string Msg { get; set; }
    }
}
