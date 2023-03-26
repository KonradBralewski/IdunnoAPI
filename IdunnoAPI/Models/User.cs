using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace IdunnoAPI.Models
{
    [Table("Users")]
    public class User
    {
        [Key][Required] public int UserId { get; set; }

        [Required] public string Username { get; set; }

        [Required] public string Password { get; set; }

        [Required] public string Role { get; private set; } = "User";
    }
}
