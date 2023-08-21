using Microsoft.AspNetCore.Mvc;
using PartyRoom.Domain.Interfaces.Services;

namespace PartyRoom.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IUserService _userService;
        public RoleController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            try
            {
                await _userService.CreateRoleAsync(roleName);
                return Ok("Роль создана");
            }
            catch
            {
                return BadRequest("Ошибка при создании роли");
            }
        }
    }
}
