using IdunnoAPI.Auth.Interfaces;
using IdunnoAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdunnoAPI.Auth
{
    public class JWToken : IJWToken // if we will get to point with more functionality, consider creating interface and using DI.
    {
        private readonly IConfiguration _cfg;

        public JWToken(IConfiguration cfg)
        {
            _cfg = cfg; 
        }

        public string GenerateToken(User user)
        {
            SymmetricSecurityKey ssk = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cfg["JWT:Key"]));
            SigningCredentials sc = new SigningCredentials(ssk, SecurityAlgorithms.HmacSha256);

            Claim[] claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            JwtSecurityToken st = new JwtSecurityToken(issuer: null, audience: null,
                claims, notBefore: null, expires: DateTime.Now.AddMinutes(Convert.ToDouble(_cfg["JWT:Expires"])), sc);

            String token = new JwtSecurityTokenHandler().WriteToken(st);

            return token;
        }

        public void SpreadToken(string token, HttpResponse response)
        {
            response.Cookies.Append(_cfg["JWT:StoringCookie"], token, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddMinutes(Convert.ToDouble(_cfg["JWT:Expires"])),
                IsEssential = true
            });
        }
    }
}
