using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PartyRoom.Contracts.DTOs.Room;
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
            if (userRooms == null)
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
            if (!await _userRoomRepository.ExistsUserInRoomAsync(userId, roomId))
            {
                throw new InvalidOperationException(ExceptionMessages.SearchFailed);
            }
            if (!await _userRoomRepository.DeleteUserInRoom(userId, roomId))
            {
                throw new InvalidOperationException(ExceptionMessages.DeletionFailed);
            }
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
            if (await _userRoomRepository.ExistsUserInRoomAsync(userId, room.Id))
            {
                throw new InvalidOperationException(ExceptionMessages.CreationFailed);
            }

            var userRoom = new UserRoom { Room = room, User = userFind };
            if (!(await _userRoomRepository.CreateAsync(userRoom)))
            {
                throw new InvalidOperationException(ExceptionMessages.CreationFailed);
            }
        }

        public async Task CheckStartRoomsAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var currentDate = DateTime.UtcNow;

                var roomsToProcess = await _roomRepository.Rooms
                    .Where(room => room.StartDate <= currentDate && room.IsStarted == false)
                    .ToListAsync(stoppingToken);

                foreach (var room in roomsToProcess)
                {
                    // Устанавливаем что комната запущена
                    room.IsStarted = true;
                    // Формируем пользователям их дарящих
                    await FormationDestinationUser(room.Id);
                    await _roomRepository.UpdateAsync(room);
                }

                // Ожидание некоторое время перед следующей проверкой 
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
        public async Task FormationDestinationUser(Guid roomId)
        {
            var userRooms = await _userRoomRepository.UserRooms.Where(ur => ur.RoomId == roomId).ToListAsync();
            var users = userRooms.Select(u => u.UserId);

            List<Guid> availableDestinations = new List<Guid>(userRooms.Count);
            foreach (var userRoom in userRooms)
            {
                availableDestinations.Add(userRoom.UserId);
            }
            Random random = new Random();
            foreach (var userRoom in userRooms)
            {
                int randomIndex = random.Next(availableDestinations.Count);
                // Исключаем возможность выбора текущего UserId в качестве DestinationUserId
                if (userRoom.UserId == availableDestinations[randomIndex])
                {
                    randomIndex = (randomIndex + 1) % availableDestinations.Count;
                }
                availableDestinations.RemoveAt(randomIndex);
            }
            if (!await _userRoomRepository.UpdateAsync(userRooms))
            {
                throw new InvalidOperationException(ExceptionMessages.UpdateFailed);
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

        public async Task CreateTestRoomAsync()
        {
            var author = _userManager.Users.FirstOrDefault();
            var room = new Room
            {
                Author = author,
                AuthorId = author.Id,
                Name = "Тестовая комната",
                Price = 500,
                Type = "Private",
                StartDate = DateTime.Now.AddMinutes(3),
                FinishDate = DateTime.Now.AddMinutes(20)
            };
            string roomSlug = await GenerateUniqueSlug();
            room.Link = roomSlug;
            await _roomRepository.CreateAsync(room);

            var users = _userManager.Users.ToList();
            users.Remove(author);
            foreach (var user in users)
            {
                await _userRoomRepository.CreateAsync(new UserRoom { Room = room, UserId = user.Id });
            }
        }

        public async Task<RoomInfoDTO> GetRoomAsync(Guid roomId)
        {
            if (roomId == Guid.Empty)
            {
                throw new ArgumentNullException();
            }
            var room = await _roomRepository.GetByIdAsync(roomId);
            if (room == null)
            {
                throw new InvalidOperationException(ExceptionMessages.SearchFailed);
            }
            var roomMap = _mapper.Map<RoomInfoDTO>(room);
            roomMap.QuantityUsers = _userRoomRepository.UserRooms.Where(r => r.RoomId == roomId).Count();

            if (roomMap == null)
            {
                throw new InvalidOperationException(ExceptionMessages.MappingFailed);
            }
            return roomMap;
        }

        public async Task<RoomDto> GetRoomAsync(string link)
        {
            if (string.IsNullOrEmpty(link))
            {
                throw new ArgumentNullException();
            }
            var room = await _roomRepository.GetRoomByLinkAsync(link);
            if (room == null)
            {
                throw new InvalidOperationException(ExceptionMessages.SearchFailed);
            }
            var roomMap = _mapper.Map<RoomDto>(room);
            if (roomMap == null)
            {
                throw new InvalidOperationException(ExceptionMessages.MappingFailed);
            }
            return roomMap;
        }

        public async Task<RoomInfoDTO> GetRoomInfoAsync(Guid roomId)
        {
            if (roomId == Guid.Empty)
            {
                throw new ArgumentNullException();
            }
            var room = await _roomRepository.GetByIdAsync(roomId);
            if (room == null)
            {
                throw new InvalidOperationException(ExceptionMessages.SearchFailed);
            }
            var roomMap = _mapper.Map<RoomInfoDTO>(room);
            roomMap.QuantityUsers = _userRoomRepository.UserRooms.Where(r => r.RoomId == roomId).Count();

            if (roomMap == null)
            {
                throw new InvalidOperationException(ExceptionMessages.MappingFailed);
            }
            return roomMap;
        }


    }
}
