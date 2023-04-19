using Azure;
using IdunnoAPI.Auth;
using IdunnoAPI.Auth.Interfaces;
using IdunnoAPI.DAL.Repositories;
using IdunnoAPI.DAL.Repositories.Interfaces;
using IdunnoAPI.DAL.Services.Interfaces;
using IdunnoAPI.Helpers;
using IdunnoAPI.Helpers.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using IdunnoAPI.Models.Users;
using IdunnoAPI.Models.Posts;

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

        public async Task<KeyValuePair<UserDTO, IEnumerable<Post>>> GetUserProfileByIdAsync(int userId)
        {
            UserDTO user = await Users.GetUserByIdAsync(userId);
            IEnumerable<Post> posts = await _posts.GetPostsAsQueryable().Where(p => p.UserId == userId).ToListAsync();

            if (posts == null) throw new RequestException(StatusCodes.Status404NotFound, "Couldn't find user posts.");

            return new KeyValuePair<UserDTO, IEnumerable<Post>>(user, posts);
        }

        public async Task<bool> ChangeUserPasswordAsync(ChangePasswordRequest cpr)
        {
            User foundUser = await Users.FindUserAsync(u => u.UserId == cpr.UserId);

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
