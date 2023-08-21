using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PartyRoom.Contracts.DTOs.User;
using PartyRoom.Domain;
using PartyRoom.Domain.Entities;
using PartyRoom.Domain.Interfaces.Services;

using PartyRoom.WebAPI.Services;

namespace PartyRoom.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtService _jwtService;
        private readonly IAccountService _accountService;
        public AccountController(IUserService userService, UserManager<ApplicationUser> userManager, JwtService jwtService, IAccountService accountService)
        {
            _userService = userService;
            _userManager = userManager;
            _jwtService = jwtService;
            _accountService = accountService;

        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDTO loginModel)
        {
            try
            {
                var user = await _userService.LoginAsync(loginModel);
                var claims = await _userManager.GetClaimsAsync(user);
                var accessToken = _jwtService.GetToken(user, claims);

                var refreshToken = _jwtService.GenerateRefreshToken();
                refreshToken.ApplicationUserId = user.Id;

                await _accountService.CreateRefreshTokenAsync(refreshToken);
                SetRefreshToken(refreshToken);
                return Ok(accessToken);
            }
            catch
            {
                return BadRequest();
            }

        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegistrationDTO model)
        {
            try
            {
                await _userService.CreateUserAsync(model);
                return await Login(new UserLoginDTO { Email = model.Email, Password = model.Password });
            }
            catch (ArgumentNullException)
            {
                return BadRequest("Параметры запроса некорректны.");
            }
            catch (InvalidOperationException ex) when (ex.Message == ExceptionMessages.MappingFailed)
            {
                return StatusCode(StatusCodes.Status501NotImplemented, "Ошибка при маппинге данных.");
            }
            catch (InvalidOperationException ex) when (ex.Message == ExceptionMessages.DuplicateItem)
            {
                return StatusCode(StatusCodes.Status409Conflict, "Пользователь с таким именем уже существует.");
            }
            catch (InvalidOperationException ex) when (ex.Message == ExceptionMessages.CreationFailed)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Не удалось создать пользователя.");
            }
            catch (InvalidOperationException ex) when (ex.Message == ExceptionMessages.SearchFailed)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Не удалось найти роль 'User'.");
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Произошла ошибка при создании пользователя.");
            }
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var token = Request.Cookies["refreshToken"];
            var currentRefreshToken = await _accountService.GetRefreshTokenAsync(token);
            if (currentRefreshToken == null)
            {
                return BadRequest();
            }
            var user = await _userManager.FindByIdAsync(currentRefreshToken.ApplicationUserId.ToString());

            if (currentRefreshToken.Expires < DateTime.Now)
            {
                var newRefreshToken = _jwtService.GenerateRefreshToken();
                newRefreshToken.ApplicationUserId = currentRefreshToken.ApplicationUserId;
                await _accountService.UpdateRefreshTokenAsync(newRefreshToken);
                SetRefreshToken(newRefreshToken);

            }

            var claims = await _userManager.GetClaimsAsync(user);
            var accessToken = _jwtService.GetToken(user, claims);
            return Ok(accessToken);
        }


        private void SetRefreshToken(RefreshToken newRefreshToken)
        {
            var cookieOprions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires,
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOprions);

        }
    }
}
