using IdunnoAPI.DAL.Repositories.Interfaces;
using IdunnoAPI.DAL.Services;
using IdunnoAPI.DAL.Services.Interfaces;
using IdunnoAPI.Extensions;
using IdunnoAPI.Models.Posts;
using IdunnoAPI.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdunnoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : BaseIdunnoController
    {
        private readonly IUserRepository _users;
        private readonly IUsersService _usersService;
        public UsersController(IUserRepository users, IUsersService usersService)
        {
            _users = users;
            _usersService = usersService;
        }

        [Route("ByUsername")]
        [HttpGet]
        public async Task<ActionResult> GetUsersByUsernameAsync([FromQuery] string username)
        {
            IEnumerable<UserDTO> users = await _users.GetUsersByUsernameAsync(username);

            return Ok(users);
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<ActionResult> GetUserByIdAsync([FromRoute]int userId)
        {
            return Ok(await _users.GetUserByIdAsync(userId));
        }

        [HttpGet]
        [Route("CurrentUser")]
        public async Task<ActionResult> GetCurrentUserAsync()
        {
            return Ok(await _users.GetUserByIdAsync(this.GetCallerId()));
        }

        [HttpGet]
        [Route("{userId}/Profile")]
        public async Task<ActionResult> GetUserProfileByIdAsync([FromRoute] int userId)
        {
            return Ok(await _usersService.GetUserProfileByIdAsync(userId));
        }

        [HttpGet]
        [Route("CurrentUser/Profile")]
        public async Task<ActionResult> GetCurrentUserProfileByIdAsync()
        {
            return Ok(await _usersService.GetUserProfileByIdAsync(this.GetCallerId()));
        }

        [HttpPost]
        [Route("CurrentUser")]
        public async Task<ActionResult> ChangeCurrentUserPasswordAsync(ChangePasswordRequest cpr)
        {
            cpr.UserId = this.GetCallerId();
            await _usersService.ChangeUserPasswordAsync(cpr);

            foreach(string cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }

            return Ok("Password has been updated.");
        }

    }
}
