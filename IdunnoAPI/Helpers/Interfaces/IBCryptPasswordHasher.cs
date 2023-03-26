using Microsoft.AspNetCore.Identity;

namespace IdunnoAPI.Helpers.Interfaces
{
    public interface IBCryptPasswordHasher
    {
        string HashPassword(string password);
        PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword);
    }
}
