using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PartyRoom.Contracts.DTOs.Room;
using PartyRoom.Domain;
using PartyRoom.Domain.Interfaces.Services;
using PartyRoom.WebAPI.Services;

namespace PartyRoom.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly JwtService _jwtService;
        public RoomController(JwtService jwtService, IRoomService roomService)
        {
            _jwtService = jwtService;
            _roomService = roomService;
        }

        [HttpPost]
        [Authorize(RoleConstants.RoleUser)]
        public async Task<IActionResult> Post(RoomCreateDTO createModel)
        {
            var userId = _jwtService.GetUserIdByToken(HttpContext);
            try
            {
                await _roomService.CreateRoomAsync(createModel, userId);
                return StatusCode(StatusCodes.Status200OK, "Комната создана");
            }
            catch (ArgumentNullException)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Не удалось создать комнату, model is null");
            }
            catch (InvalidOperationException ex) when (ex.Message == ExceptionMessages.SearchFailed)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Нет такого пользователя");
            }
            catch (InvalidOperationException ex) when (ex.Message == ExceptionMessages.MappingFailed)
            {
                return StatusCode(StatusCodes.Status501NotImplemented, "Ошибка сервера, не удалось смаппить данные");
            }
            catch (InvalidOperationException ex) when (ex.Message == ExceptionMessages.CreationFailed)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Не удалось создать комнату");
            }
        }

        [HttpPost("ConnectToRoom")]
        [Authorize(RoleConstants.RoleUser)]
        public async Task<IActionResult> ConnectToRoom([FromQuery] string link)
        {
            var userId = _jwtService.GetUserIdByToken(HttpContext);
            try
            {
                await _roomService.JoinToRoomAsync(link, userId);
                return Ok("Удалось подключиться к комнате");
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }
            catch (InvalidOperationException ex) when (ex.Message == ExceptionMessages.SearchFailed)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex) when (ex.Message == ExceptionMessages.CreationFailed)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Authorize(RoleConstants.RoleUser)] // TODO: Добавить нормальный GET запрос
        public async Task<IActionResult> Get([FromQuery] string? link, [FromQuery] Guid? id)
        {
            if (id != Guid.Empty && id != null)
            {
                var roomById = await _roomService.GetRoomInfoAsync(id.Value);
                return Ok(roomById);
            }
            else if (!string.IsNullOrEmpty(link))
            {
                var roomByLink = await _roomService.GetRoomAsync(link);
                return Ok(roomByLink);
            }
            else
            {
                return BadRequest("Invalid parameters");
            }
        }


        [HttpDelete("DeleteUserFromRoomById")]
        public async Task<IActionResult> DeleteUserFromRoom(Guid userId, Guid roomId)
        {
            try
            {
                await _roomService.DisconnectUserFromRoom(userId, roomId);
                return Ok();
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex) when (ex.Message == ExceptionMessages.SearchFailed)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex) when (ex.Message == ExceptionMessages.DeletionFailed)
            {
                return BadRequest();
            }
        }

        [HttpDelete("DisconnectFromRoom")]
        public async Task<IActionResult> DisconnectFromRoom(Guid roomId)
        {
            var userId = _jwtService.GetUserIdByToken(HttpContext);
            try
            {
                await _roomService.DisconnectUserFromRoom(userId, roomId);
                return Ok();
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex) when (ex.Message == ExceptionMessages.SearchFailed)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex) when (ex.Message == ExceptionMessages.DeletionFailed)
            {
                return BadRequest();
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid roomId)
        {
            try
            {
                await _roomService.DeleteRoomAsync(roomId);
                return Ok();
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex) when (ex.Message == ExceptionMessages.SearchFailed)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex) when (ex.Message == ExceptionMessages.DeletionFailed)
            {
                return BadRequest();
            }
        }

        [HttpPost("CreateTestRoom")]
        public async Task<IActionResult> CreateTestRoom()
        {
            await _roomService.CreateTestRoomAsync();
            return Ok();
        }
    }
}