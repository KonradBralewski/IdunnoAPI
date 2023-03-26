using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace IdunnoAPI.Models
{
    [Table("Posts")]
    public class Post
    {
        
        [Key][Required] public int PostId { get; set; }
        [Required] public int UserId { get; set; }
        [Required] public string PostDate { get; private set; } = DateTime.Now.ToString("yyyy-MM-dd H:mm");
        [Required] public string PostTitle { get; set; }
        [Required] public string PostDescription { get; set; }

        public string ImagePath { get; set; }
    }
}
