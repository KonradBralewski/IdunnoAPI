using Azure;
using IdunnoAPI.Auth;
using IdunnoAPI.Auth.Interfaces;
using IdunnoAPI.DAL.Repositories;
using IdunnoAPI.DAL.Repositories.Interfaces;
using IdunnoAPI.DAL.Services.Interfaces;
using IdunnoAPI.Helpers;
using IdunnoAPI.Helpers.Interfaces;
using IdunnoAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;

namespace IdunnoAPI.DAL.Services
{
    public class UsersService : IUsersService, IDisposable
    {
        private bool disposedValue;
        public IUserRepository Users { get; }
        private readonly IJWToken _tk;
        private readonly IBCryptPasswordHasher _passwordHasher;


        public UsersService(IUserRepository users, IJWToken tokenGenerator, IBCryptPasswordHasher passwordHasher) 
        {
            Users = users;
            _tk = tokenGenerator;
            _passwordHasher = passwordHasher;
        }


        public async Task<bool> RegisterUserAsync(User user)
        {
            user.Password = _passwordHasher.HashPassword(user.Password);
            return await Users.AddUserAsync(user);
        }
        public async Task<string> AuthenticateUser(User user, HttpResponse response)
        {
            User foundUser = await Users.FindUserAsync(u => u.Username == user.Username);

            PasswordVerificationResult pvr = foundUser != null ? _passwordHasher.VerifyHashedPassword(foundUser.Password, user.Password) : PasswordVerificationResult.Failed;

            if (foundUser == null || pvr == PasswordVerificationResult.Failed) throw new RequestException(StatusCodes.Status401Unauthorized, "Provided credentials are invalid.");

            string token = _tk.GenerateToken(foundUser);

            _tk.SpreadToken(token, response);

            return token;
        }

        public async Task<string> GetUserNameAsync(int userId)
        {
            User foundUser = await Users.FindUserAsync(u => u.UserId == userId);

            if (foundUser == null) throw new RequestException(StatusCodes.Status404NotFound, "User could not be found.");

            return foundUser.Username;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                 if(Users!= null)
                    {
                        Users.Dispose();
                    }   
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

       
    }
}
