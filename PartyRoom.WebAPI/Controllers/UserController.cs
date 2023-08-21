using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PartyRoom.Domain;
using PartyRoom.Domain.Entities;
using PartyRoom.Domain.Interfaces.Services;

using PartyRoom.WebAPI.Services;
using System.Security.Cryptography;

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

        //[HttpPost("register")]
        //public async Task<IActionResult> Register(UserRegistrationDTO model)
        //{
        //    try
        //    {
        //        await _userService.CreateUserAsync(model);

        //        return StatusCode(StatusCodes.Status202Accepted, "Пользователь успешно создан.");
        //    }
        //    catch (ArgumentNullException)
        //    {
        //        return BadRequest("Параметры запроса некорректны.");
        //    }
        //    catch (InvalidOperationException ex) when (ex.Message == ExceptionMessages.MappingFailed)
        //    {
        //        return StatusCode(StatusCodes.Status501NotImplemented, "Ошибка при маппинге данных.");
        //    }
        //    catch (InvalidOperationException ex) when (ex.Message == ExceptionMessages.DuplicateItem)
        //    {
        //        return StatusCode(StatusCodes.Status409Conflict, "Пользователь с таким именем уже существует.");
        //    }
        //    catch (InvalidOperationException ex) when (ex.Message == ExceptionMessages.CreationFailed)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, "Не удалось создать пользователя.");
        //    }
        //    catch (InvalidOperationException ex) when (ex.Message == ExceptionMessages.SearchFailed)
        //    {
        //        return StatusCode(StatusCodes.Status404NotFound, "Не удалось найти роль 'User'.");
        //    }
        //    catch (InvalidOperationException ex)
        //    {
        //        return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, "Произошла ошибка при создании пользователя.");
        //    }
        //}

        //[HttpPost("Login")]
        //public async Task<IActionResult> Login(UserLoginDTO loginModel)
        //{
        //    try
        //    {
        //        var user = await _userService.LoginAsync(loginModel);
        //        var claims = await _userManager.GetClaimsAsync(user);
        //        var token = _jwtService.GetToken(user, claims);
        //        var refreshToken = GenerateRefreshToken();
        //        SetRefreshToken(refreshToken);
        //        //return Ok(new { Token = token });
        //        return Ok(token);
        //    }
        //    catch (ArgumentNullException)
        //    {
        //        return BadRequest("Параметры запроса некорректны.");
        //    }
        //    catch (InvalidOperationException ex) when (ex.Message == ExceptionMessages.SearchFailed)
        //    {
        //        return StatusCode(400, "Не удалось найти пользователя с таким логином и паролем");
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(500, "Произошла ошибка при создании токена");
        //    }
        //}
        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(65)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };
            return refreshToken;

        }
        private void SetRefreshToken(Domain.Entities.RefreshToken newRefreshToken)
        {
            var cookieOprions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires,
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOprions);

        }
        [HttpDelete]
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

        [HttpPost("CreateTestUsers")]
        public async Task<IActionResult> CreateTestUsers()
        {
            await _userService.CreateTestUsers();
            return Ok("Тестовые пользователи загрузились");
        }
    }
}

