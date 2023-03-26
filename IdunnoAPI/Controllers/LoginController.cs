using IdunnoAPI.DAL.Services.Interfaces;
using IdunnoAPI.Models;
using Microsoft.AspNetCore.Mvc;


namespace IdunnoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUsersService _usersService;
        public LoginController(IUsersService usersService)
        {
            _usersService = usersService;
        }


        [HttpPost]
        public async Task<ActionResult> LoginAsync([FromBody] User user)
        {
            string token = await _usersService.AuthenticateUser(user, Response);

            return Ok(token);
        }


    }
}
