using Microsoft.AspNetCore.Mvc;
using PartyRoom.Domain;
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
            try
            {
                var user = await _profileService.GetCurrentProfileAsync(userId);
                return Ok(user);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest("Некорректный пользователь");
            }
            catch (InvalidOperationException ex) when (ex.Message == ExceptionMessages.SearchFailed)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex) when (ex.Message == ExceptionMessages.MappingFailed)
            {
                return BadRequest("Ошибка сервера");
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(Guid userId)
        {
            try
            {
                var user = await _profileService.GetPublicUserProfileAsync(userId);
                return Ok(user);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest("Некорректный пользователь");
            }
            catch (InvalidOperationException ex) when (ex.Message == ExceptionMessages.SearchFailed)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex) when (ex.Message == ExceptionMessages.MappingFailed)
            {
                return BadRequest("Ошибка сервера");
            }
        }
    }
}
