using IdunnoAPI.Controllers;
using IdunnoAPI.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace IdunnoAPI.Extensions
{
    public abstract class BaseIdunnoController : ControllerBase
    {
        protected int GetCallerId()
        {
            Claim userId = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userId != null)
            {
                return Int32.Parse(userId.Value);
            }

            throw new RequestException(StatusCodes.Status500InternalServerError, "Couldn't receive information about request sending user.");
        }

        protected string HashString(string toBeHashed)
        {
            return "";
        }
    }
}
