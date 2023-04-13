using IdunnoAPI.Models.Posts;
using System.ComponentModel.DataAnnotations;

namespace IdunnoAPI.Models.Users
{
    public class UserDTO
    {
        [Required] public int UserId { get; set; }
        [Required] public string Username { get; set; }
        [Required] public string Role { get; set; }
    }
}
