using IdunnoAPI.DAL.Repositories.Interfaces;
using IdunnoAPI.DAL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdunnoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
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
        public async Task<ActionResult> GetNameByUserIdAsync([FromRoute]int userId)
        {
            return Ok(await _usersService.GetUserNameAsync(userId));
        }
    }
}
