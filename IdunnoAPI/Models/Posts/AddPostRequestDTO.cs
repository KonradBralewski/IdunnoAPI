using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace IdunnoAPI.Models.Posts
{
    public class AddPostRequestDTO
    {
        [Required] public string PostTitle { get; set; }
        [Required] public string PostDescription { get; set; }

    }
}
