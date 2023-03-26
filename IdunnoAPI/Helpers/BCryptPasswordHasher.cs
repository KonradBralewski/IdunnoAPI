using IdunnoAPI.Helpers.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace IdunnoAPI.Helpers
{
    public class BCryptPasswordHasher : IBCryptPasswordHasher
    {
        public string HashPassword(string password)
        {
            if(string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(password));

            return BCrypt.Net.BCrypt.HashPassword(password, 11, false);
        }

        /// <summary>
        ///  Verifies password, PasswordsNeedRehash not used as we will just keep constant workFactor for demo project purpose.
        /// </summary>

        public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            if(string.IsNullOrEmpty(hashedPassword)) throw new Exception(nameof(hashedPassword));
            if(string.IsNullOrEmpty(providedPassword)) throw new Exception(nameof(providedPassword));

            bool isValid = BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);

            return isValid ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }
    }
}
