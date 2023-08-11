using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PartyRoom.Domain;
using System.IdentityModel.Tokens.Jwt;

namespace PartyRoom.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        [Authorize(RoleConstants.RoleUser)]
        [HttpGet]
        public IActionResult Test()
        {
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var token = authorizationHeader?.Replace("Bearer ", "");

            if (token != null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                var role = jwtToken.Claims.First(claim => claim.Type == "Role").Value;
                var userId = jwtToken.Claims.First(claim => claim.Type == "Id").Value;

                return Ok(new { Role = role, UserId = userId });
            }
            return Ok("хуйня пришла из токена");
        }
    }
}
