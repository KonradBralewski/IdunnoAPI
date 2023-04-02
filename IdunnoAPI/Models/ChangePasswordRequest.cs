using System.ComponentModel.DataAnnotations;

namespace IdunnoAPI.Models
{
    public class ChangePasswordRequest
    {
        [Required] public int UserId { get; set; }
        [Required] public string CurrentPassword { get; set; }
        [Required] public string NewPassword { get; set; }
    }
}
