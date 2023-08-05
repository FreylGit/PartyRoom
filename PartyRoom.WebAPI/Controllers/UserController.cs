using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PartyRoom.Contracts.DTOs.User;
using PartyRoom.Domain;
using PartyRoom.Domain.Entities;
using PartyRoom.Domain.Interfaces;
using PartyRoom.WebAPI.Services;

namespace PartyRoom.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;
        private readonly JwtService _jwtService;
        public UserController(UserManager<ApplicationUser> userManager, JwtService jwtService, IUserService userService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegistrationDTO model)
        {
            try
            {
                await _userService.CreateUserAsync(model);
                return Ok("Пользователь успешно создан.");
            }
            catch (ArgumentNullException)
            {
                return BadRequest("Параметры запроса некорректны.");
            }
            catch (InvalidOperationException ex) when (ex.Message == ExceptionMessages.MappingFailed)
            {
                return StatusCode(400, "Ошибка при маппинге данных.");
            }
            catch (InvalidOperationException ex) when (ex.Message == ExceptionMessages.DuplicateItem)
            {
                return StatusCode(409, "Пользователь с таким именем уже существует.");
            }
            catch (InvalidOperationException ex) when (ex.Message == ExceptionMessages.CreationFailed)
            {
                return StatusCode(500, "Не удалось создать пользователя.");
            }
            catch (InvalidOperationException ex) when (ex.Message == ExceptionMessages.SearchFailed)
            {
                return StatusCode(500, "Не удалось найти роль 'User'.");
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Произошла ошибка при создании пользователя.");
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDTO loginModel)
        {
            try
            {
                var user = await _userService.LoginAsync(loginModel);
                var claims = await _userManager.GetClaimsAsync(user);
                var token = _jwtService.GetToken(user, claims);
                return Ok(new { Token = token });
            }
            catch (ArgumentNullException)
            {
                return BadRequest("Параметры запроса некорректны.");
            }
            catch (InvalidOperationException ex) when (ex.Message == ExceptionMessages.SearchFailed)
            {
                return StatusCode(400,"Не удалось найти пользователя с таким логином и паролем");
            }
            catch (Exception)
            {
                return StatusCode(500, "Произошла ошибка при создании токена");
            }
        }
 
        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _userService.DeleteUserByIdAsync(id);
                return Ok("Пользователь удален");
            }
            catch (ArgumentNullException)
            {
                return BadRequest("Параметры запроса некорректны.");
            }
            catch (InvalidOperationException ex) when (ex.Message == ExceptionMessages.SearchFailed)
            {
                return StatusCode(404, "Не удалось найти пользователя с таким id");
            }
            catch (Exception)
            {
                return StatusCode(500, "Произошла ошибка при удалении пользователя");
            }
        }
    }
}

