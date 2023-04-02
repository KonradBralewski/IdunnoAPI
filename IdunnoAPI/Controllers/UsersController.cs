using IdunnoAPI.DAL.Repositories.Interfaces;
using IdunnoAPI.DAL.Services.Interfaces;
using IdunnoAPI.Extensions;
using IdunnoAPI.Models;
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

        [HttpGet]
        [Route("{userId}")]
        public async Task<ActionResult> GetUserByIdAsync([FromRoute]int userId)
        {
            return Ok(await _usersService.GetUserByIdAsync(userId));
        }

        [HttpGet]
        [Route("CurrentUser")]
        public async Task<ActionResult> GetCurrentUser()
        {
            return Ok(await _usersService.GetUserByIdAsync(this.GetCallerId()));
        }

        [HttpPost]
        [Route("CurrentUser")]
        public async Task<ActionResult> ChangeCurrentUserPassword(ChangePasswordRequest cpr)
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
