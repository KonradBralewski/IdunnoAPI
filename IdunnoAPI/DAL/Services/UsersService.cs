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
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace IdunnoAPI.DAL.Services
{
    public class UsersService : IUsersService, IDisposable
    {
        private bool disposedValue;
        public IUserRepository Users { get; }
        private readonly IPostRepository _posts;
        private readonly IJWToken _tk;
        private readonly IBCryptPasswordHasher _passwordHasher;


        public UsersService(IUserRepository users, IJWToken tokenGenerator, IBCryptPasswordHasher passwordHasher, IPostRepository Posts) 
        {
            Users = users;
            _tk = tokenGenerator;
            _passwordHasher = passwordHasher;
            _posts = Posts;
        }


        public async Task<bool> RegisterUserAsync(User user)
        {
            user.Password = _passwordHasher.HashPassword(user.Password);
            return await Users.AddUserAsync(user);
        }
        public async Task<string> AuthenticateUser(User user, HttpResponse response)
        {
            User foundUser; 
            try
            {
                foundUser = await Users.FindUserAsync(u => u.Username == user.Username);
            }
            catch(RequestException re) // only when user could not be found
            {
                throw new RequestException(StatusCodes.Status401Unauthorized, "Provided credentials are invalid.");
            }

            PasswordVerificationResult pvr = _passwordHasher.VerifyHashedPassword(foundUser.Password, user.Password);

            if (pvr == PasswordVerificationResult.Failed) throw new RequestException(StatusCodes.Status401Unauthorized, "Provided credentials are invalid.");

            string token = _tk.GenerateToken(foundUser);

            _tk.SpreadToken(token, response);

            return token;
        }

        public async Task<UserProfile> GetUserByIdAsync(int userId)
        {
            User foundUser = await Users.FindUserAsync(u => u.UserId == userId);

            UserProfile userProfile = new UserProfile
            {
                Username = foundUser.Username,
                Role = foundUser.Role,
                UserPosts = await _posts.GetPostsAsQueryable().Where(p => p.UserId == userId).ToListAsync()
            };

            return userProfile;
        }

        public async Task<bool> ChangeUserPasswordAsync(ChangePasswordRequest cpr)
        {
            User foundUser = await Users.FindUserAsync(cpr.UserId);

            PasswordVerificationResult pvr = _passwordHasher.VerifyHashedPassword(foundUser.Password, cpr.CurrentPassword);

            if (pvr == PasswordVerificationResult.Failed) throw new RequestException(StatusCodes.Status401Unauthorized, "Provided password is invalid.");

            cpr.NewPassword = _passwordHasher.HashPassword(cpr.NewPassword);

            return await Users.ChangeUserPasswordAsync(cpr);
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
