using IdunnoAPI.Models.Posts;

namespace IdunnoAPI.Models.Users
{
    public class UserProfile // not actual DB model, will be used in GET UsersController methods
    {
        public string Username { get; set; }
        public string Role { get; set; }
        public IEnumerable<Post> UserPosts { get; set; }
    }
}
