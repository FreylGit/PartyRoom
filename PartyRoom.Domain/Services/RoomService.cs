using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PartyRoom.Contracts.DTOs.Room;
using PartyRoom.Contracts.DTOs.User;
using PartyRoom.Domain.Entities;
using PartyRoom.Domain.Interfaces.Repository;
using PartyRoom.Domain.Interfaces.Services;

namespace PartyRoom.Domain.Services
{
    public class RoomService : IRoomService
    {
        private readonly IMapper _mapper;
        private readonly IRoomRepository _roomRepository;
        private readonly IUserRoomRepository _userRoomRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public RoomService(IMapper mapper, IRoomRepository roomRepository, IUserRoomRepository userRoomRepository,
            UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _roomRepository = roomRepository;
            _userRoomRepository = userRoomRepository;
            _userManager = userManager;
        }
        public async Task CreateRoomAsync(RoomCreateDTO createModel, Guid authorId)
        {
            if (createModel == null || authorId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(createModel) + " " + nameof(authorId));
            }
            var author = await _userManager.FindByIdAsync(authorId.ToString());
            if (author == null)
            {
                throw new InvalidOperationException(ExceptionMessages.SearchFailed);
            }
            var roomMap = _mapper.Map<Room>(createModel);
            if (roomMap == null)
            {
                throw new InvalidOperationException(ExceptionMessages.MappingFailed);
            }
            roomMap.Author = author;
            roomMap.AuthorId = author.Id;
            // Генерация уникальной ссылки
            string roomSlug = await GenerateUniqueSlug();
            roomMap.Link = roomSlug;
            if (!(await _roomRepository.CreateAsync(roomMap)))
            {
                throw new InvalidOperationException(ExceptionMessages.CreationFailed);
            }
        }

        public async Task DeleteRoomAsync(Guid roomId)
        {
            if (!await _roomRepository.ExistsIdAsync(roomId))
            {
                throw new InvalidOperationException(ExceptionMessages.SearchFailed);
            }

            var userRooms = _userRoomRepository.GetUserRooms(roomId);
            if (userRooms ==null)
            {
                throw new InvalidOperationException(ExceptionMessages.SearchFailed);
            }

            if (!await _userRoomRepository.DeleteUserRoomsAsync(userRooms))
            {
                throw new InvalidOperationException(ExceptionMessages.DeletionFailed);
            }
            var room = await _roomRepository.GetByIdAsync(roomId);
            if (room == null)
            {
                throw new InvalidOperationException(ExceptionMessages.SearchFailed);
            }
            if (!await _roomRepository.DeleteAsync(room))
            {
                throw new InvalidOperationException(ExceptionMessages.DeletionFailed);
            }
        }

        public async Task DisconnectUserFromRoom(Guid userId, Guid roomId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userId));
            }
            if (!await _userRoomRepository.ExistsUserInRoomAsync(userId,roomId))
            {
                throw new InvalidOperationException(ExceptionMessages.SearchFailed);
            }
            if (!await _userRoomRepository.DeleteUserInRoom(userId, roomId))
            {
                throw new InvalidOperationException(ExceptionMessages.DeletionFailed);
            }
        }

        public async Task<IEnumerable<RoomDto>> GetRoomsByUserIdAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userId));
            }
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new InvalidOperationException(ExceptionMessages.SearchFailed);
            }
            var rooms = await _userRoomRepository.GetRoomsByUserIdAsync(userId);
            var roomsMap = _mapper.Map<IEnumerable<RoomDto>>(rooms);
            if (roomsMap == null)
            {
                throw new InvalidOperationException(ExceptionMessages.MappingFailed);
            }
            return roomsMap;
        }

        public async Task<IEnumerable<PublicUserDTO>> GetUsersByRoomAsync(Guid roomId)
        {
            if (roomId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(roomId));
            }
            if (!await _roomRepository.ExistsIdAsync(roomId))
            {
                throw new InvalidOperationException(ExceptionMessages.SearchFailed);
            }

            var users = await _userRoomRepository.GetUsersAsync(roomId);
            var usersMap = _mapper.Map<IEnumerable<PublicUserDTO>>(users);

            if (usersMap == null)
            {
                throw new InvalidOperationException(ExceptionMessages.MappingFailed);
            }

            return usersMap;
        }

        public async Task JoinToRoomAsync(string connectLink, Guid userId)
        {
            if (string.IsNullOrEmpty(connectLink) || userId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(connectLink) + " " + nameof(userId));
            }
            // Проверка на существование ссылки в бд
            if (!await _roomRepository.ExistsLinkAsync(connectLink))
            {
                throw new InvalidOperationException(ExceptionMessages.SearchFailed);
            }

            var userFind = await _userManager.FindByIdAsync(userId.ToString());
            if (userFind == null)
            {
                throw new InvalidOperationException(ExceptionMessages.SearchFailed);
            }
            // Получаем комнату по ссылке
            var room = await _roomRepository.GetRoomByLinkAsync(connectLink);
            if (room == null)
            {
                throw new InvalidOperationException(ExceptionMessages.SearchFailed);
            }
            //Проверка eсли пользватель находитсья в комтане
            if( await _userRoomRepository.ExistsUserInRoomAsync(userId, room.Id))
            {
                throw new InvalidOperationException(ExceptionMessages.CreationFailed);
            }

            var userRoom = new UserRoom { Room = room, User = userFind };
            if (!(await _userRoomRepository.CreateAsync(userRoom)))
            {
                throw new InvalidOperationException(ExceptionMessages.CreationFailed);
            }
        }

        private async Task<string> GenerateUniqueSlug()
        {
            var length = 12;
            using var rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            var bytes = new byte[length];
            rng.GetBytes(bytes);

            string slug = Convert.ToBase64String(bytes)
                .Replace("/", "_")
                .Replace("+", "-")
                .Replace("=", "")
                .Substring(0, length);

            while (await _roomRepository.ExistsLinkAsync(slug))
            {
                rng.GetBytes(bytes);
                slug = Convert.ToBase64String(bytes)
                    .Replace("/", "_")
                    .Replace("+", "-")
                    .Replace("=", "")
                    .Substring(0, length);
            }

            return slug;
        }
    }
}
