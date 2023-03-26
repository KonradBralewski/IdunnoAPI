using IdunnoAPI.Models;
using System.IdentityModel.Tokens.Jwt;

namespace IdunnoAPI.Auth.Interfaces
{
    public interface ITokenGenerator
    {
        string GenerateToken(User user);
    }
}
