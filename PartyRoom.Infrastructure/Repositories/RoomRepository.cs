using Microsoft.EntityFrameworkCore;
using PartyRoom.Domain.Entities;
using PartyRoom.Domain.Interfaces.Repository;
using PartyRoom.Infrastructure.Data;

namespace PartyRoom.Infrastructure.Repositories
{
    /// <summary>
    /// Репозиторий для работы с комнатами.
    /// </summary>
    public class RoomRepository : IRoomRepository
    {
        private readonly ApplicationDbContext _context;
        public RoomRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Все комнаты
        /// </summary>
        public IQueryable<Room> Rooms => _context.Rooms;

        /// <summary>
        /// Создает новую комнату асинхронно.
        /// </summary>
        /// <param name="createModel">Модель создаваемой комнаты.</param>
        /// <returns>True, если операция успешна.</returns>
        public async Task<bool> CreateAsync(Room createModel)
        {
            await _context.Rooms.AddAsync(createModel);
            var userRoom = new UserRoom { Room = createModel, UserId = createModel.AuthorId };
            await _context.UserRoom.AddAsync(userRoom);
            return await SaveAsync();
        }

        /// <summary>
        /// Удаляет комнату асинхронно.
        /// </summary>
        /// <param name="deleteModel">Модель удаляемой комнаты.</param>
        /// <returns>True, если операция успешна.</returns>

        public async Task<bool> DeleteAsync(Room deleteModel)
        {
            _context.Rooms.Remove(deleteModel);
            return await SaveAsync();
        }

        /// <summary>
        /// Удаляет пользователя из комнаты асинхронно.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="roomId">Идентификатор комнаты.</param>
        /// <returns>True, если операция успешна.</returns>
        public async Task<bool> DeleteUserFromRoomAsync(Guid userId, Guid roomId)
        {
            var userRoom = await _context.UserRoom.FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoomId == roomId);
            _context.UserRoom.Remove(userRoom);
            return await SaveAsync();
        }

        /// <summary>
        /// Удаляет пользовательские комнаты асинхронно.
        /// </summary>
        /// <param name="userRooms">Коллекция пользовательских комнат.</param>
        /// <returns>True, если операция успешна.</returns>
        public async Task<bool> DeleteUserRoomsAsync(IQueryable<UserRoom> userRooms)
        {
            _context.UserRoom.RemoveRange(userRooms);
            return await SaveAsync();
        }

        /// <summary>
        /// Проверяет наличие комнаты по идентификатору асинхронно.
        /// </summary>
        /// <param name="id">Идентификатор комнаты.</param>
        /// <returns>True, если комната существует.</returns>
        public async Task<bool> ExistsIdAsync(Guid id)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(room => room.Id == id);
            if (room == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Проверяет наличие комнаты по ссылке асинхронно.
        /// </summary>
        /// <param name="connectLink">Ссылка для подключения к комнате.</param>
        /// <returns>True, если комната существует.</returns>
        public async Task<bool> ExistsLinkAsync(string connectLink)
        {
            if (await _context.Rooms.AnyAsync(r => r.Link == connectLink))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Получает все комнаты асинхронно.
        /// </summary>
        /// <returns>Коллекция всех комнат.</returns>
        public async Task<IQueryable<Room>> GetAllAsync()
        {
            return await Task.FromResult(_context.Rooms.AsQueryable().AsNoTracking());
        }

        /// <summary>
        /// Получает комнату по идентификатору асинхронно.
        /// </summary>
        /// <param name="id">Идентификатор комнаты.</param>
        /// <returns>Найденная комната.</returns>
        public async Task<Room> GetByIdAsync(Guid id)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id);
            return room;
        }

        /// <summary>
        /// Получает комнату по ссылке асинхронно.
        /// </summary>
        /// <param name="connectionLink">Ссылка для подключения к комнате.</param>
        /// <returns>Найденная комната.</returns>
        public async Task<Room> GetRoomByLinkAsync(string connectionLink)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Link == connectionLink);
            return room;
        }

        public async Task<bool> SaveAsync()
        {
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        /// <summary>
        /// Обновляет комнату асинхронно.
        /// </summary>
        /// <param name="updateModel">Модель обновляемой комнаты.</param>
        /// <returns>True, если операция успешна.</returns>
        public async Task<bool> UpdateAsync(Room updateModel)
        {
            _context.Rooms.Update(updateModel);
            return await SaveAsync();
        }
    }
}
