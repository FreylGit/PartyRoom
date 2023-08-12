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

        [HttpPost("CreateRoom")]
        [Authorize(RoleConstants.RoleUser)]
        public async Task<IActionResult> CreateRoom(RoomCreateDTO createModel)
        {
            var userId = _jwtService.GetUserIdByToken(HttpContext);
            try
            {
                await _roomService.CreateRoomAsync(createModel, userId);
                return Ok("Комната создана");
            }
            catch (ArgumentNullException)
            {
                return BadRequest("Не удалось создать комнату, model is null");
            }
            catch (InvalidOperationException ex) when (ex.Message == ExceptionMessages.SearchFailed)
            {
                return NotFound("Нет такого пользователя");
            }
            catch (InvalidOperationException ex) when (ex.Message == ExceptionMessages.MappingFailed)
            {
                return BadRequest("Ошибка сервера, не удалось смаппить данные");
            }
            catch (InvalidOperationException ex) when (ex.Message == ExceptionMessages.CreationFailed)
            {
                return BadRequest("Не удалось создать комнату");
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

        [HttpGet("GetUsersByRoomId")]
        public async Task<IActionResult> GetUsersByRoomId(Guid roomId)
        {
            try
            {
                var users = await _roomService.GetUsersByRoomAsync(roomId);
                return Ok(users);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            catch (InvalidCastException ex) when (ex.Message == ExceptionMessages.SearchFailed)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex) when (ex.Message == ExceptionMessages.MappingFailed)
            {
                return StatusCode(400, "Ошибка при маппинге данных.");
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GetRoomsByUserId")]
        public async Task<IActionResult> GetRoomsByUserId(Guid userId)
        {
            try
            {
                var rooms = await _roomService.GetRoomsByUserIdAsync(userId);
                return Ok(rooms);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex) when (ex.Message == ExceptionMessages.SearchFailed)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex) when (ex.Message == ExceptionMessages.MappingFailed)
            {
                return BadRequest();
            }
        }

        [HttpGet("GetRoomsByUser")]
        public async Task<IActionResult> GetRoomsByUser()
        {
            var userId = _jwtService.GetUserIdByToken(HttpContext);
            try
            {
                var rooms = await _roomService.GetRoomsByUserIdAsync(userId);
                return Ok(rooms);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex) when (ex.Message == ExceptionMessages.SearchFailed)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex) when (ex.Message == ExceptionMessages.MappingFailed)
            {
                return BadRequest();
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

        [HttpDelete("DeleteUserFromRoomByUser")]
        public async Task<IActionResult> DeleteUserFromRoom(Guid roomId)
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
        [HttpDelete("DeleteRoomById")]
        public async Task<IActionResult> DeletRoomById(Guid roomId)
        {
            try
            {
                await _roomService.DeleteRoomAsync(roomId);
                return Ok();
            }
            catch(ArgumentNullException)
            {
                return NotFound();
            }
            catch(InvalidOperationException ex) when(ex.Message == ExceptionMessages.SearchFailed)
            {
                return NotFound();
            }
            catch(InvalidOperationException ex) when(ex.Message == ExceptionMessages.DeletionFailed)
            {
                return BadRequest();
            }
        }
    }
}