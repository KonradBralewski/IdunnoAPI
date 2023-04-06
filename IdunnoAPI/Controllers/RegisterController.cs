using IdunnoAPI.DAL.Repositories.Interfaces;
using IdunnoAPI.DAL.Services.Interfaces;
using IdunnoAPI.Models.Users;
using Microsoft.AspNetCore.Mvc;

namespace IdunnoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IUserRepository _users;
        public RegisterController(IUsersService usersService, IUserRepository users)
        {
            _usersService = usersService;
            _users = users;
        }

        [HttpPost]

        public async Task<ActionResult> RegisterAsync([FromBody]User user) // 200 OK not Created 204 we won't return User in request response due pure security.
        {
            await _usersService.RegisterUserAsync(user);

            return Ok("User has been registered.");
        }
    }
}
