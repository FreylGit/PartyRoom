using Microsoft.AspNetCore.Mvc;
using PartyRoom.Domain.Interfaces.Services;
using PartyRoom.WebAPI.Services;

namespace PartyRoom.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtService _jwtService;
        private readonly IProfileService _profileService;
        public ProfileController(IUserService userService, JwtService jwtService, IProfileService profileService)
        {
            _userService = userService;
            _jwtService = jwtService;
            _profileService = profileService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = _jwtService.GetUserIdByToken(HttpContext);
            var user = await _profileService.GetCurrentProfile(userId);
            return Ok(user);
        }
    }
}
