using IdunnoAPI.Models.Users;
using System.IdentityModel.Tokens.Jwt;

namespace IdunnoAPI.Auth.Interfaces
{
    public interface IJWToken
    {
        string GenerateToken(User user);
        void SpreadToken(string token, HttpResponse response);
    }
}
