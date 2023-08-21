using Microsoft.EntityFrameworkCore;
using PartyRoom.Domain.Entities;
using PartyRoom.Domain.Interfaces.Repository;
using PartyRoom.Infrastructure.Data;

namespace PartyRoom.Infrastructure.Repositories
{
    public class UserRoomRepository : IUserRoomRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRoomRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Возвращает набор данных пользовательских комнат.
        /// </summary>
        public IQueryable<UserRoom> UserRooms => _context.UserRoom;

        /// <summary>
        /// Создает новую запись пользовательской комнаты асинхронно.
        /// </summary>
        /// <param name="createModel">Модель данных для создания</param>
        /// <returns>True, если операция выполнена успешно, иначе False</returns>
        public async Task<bool> CreateAsync(UserRoom createModel)
        {
            await _context.UserRoom.AddAsync(createModel);
            return await SaveAsync();
        }

        /// <summary>
        /// Удаляет запись пользовательской комнаты асинхронно.
        /// </summary>
        /// <param name="deleteModel">Модель данных для удаления</param>
        /// <returns>True, если операция выполнена успешно, иначе False</returns>
        public async Task<bool> DeleteAsync(UserRoom deleteModel)
        {
            _context.UserRoom.Remove(deleteModel);
            return await SaveAsync();
        }

        public async Task<bool> DeleteUserRoomsAsync(IQueryable<UserRoom> userRooms)
        {
            _context.UserRoom.RemoveRange(userRooms);
            return await SaveAsync();
        }

        /// <summary>
        /// Не нужно
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> ExistsIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Возвращает результат состоит ли пользователь в комнате
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public async Task<bool> ExistsUserInRoomAsync(Guid userId, Guid roomId)
        {
            var userRoom = await _context.UserRoom.FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoomId == roomId);
            if (userRoom == null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Возвращает все записи пользовательских комнат асинхронно.
        /// </summary>
        /// <returns>Набор данных пользовательских комнат</returns>
        public async Task<IQueryable<UserRoom>> GetAllAsync()
        {
            return await Task.FromResult(_context.UserRoom.AsQueryable().AsNoTracking());
        }

        /// <summary>
        /// Возвращает запись пользовательской комнаты по идентификатору асинхронно.
        /// </summary>
        /// <param name="id">Идентификатор записи</param>
        /// <returns>Запись пользовательской комнаты или null, если не найдено</returns>
        public async Task<UserRoom> GetByIdAsync(Guid id)
        {
            var userRoom = await _context.UserRoom.FirstOrDefaultAsync(ur => ur.UserId == id);
            return userRoom;
        }

        /// <summary>
        /// Возвращает пользовательские комнаты по идентификатору комнаты.
        /// </summary>
        /// <param name="roomId">Идентификатор комнаты</param>
        /// <returns>Набор пользовательских комнат</returns>
        public IQueryable<UserRoom> GetUserRooms(Guid roomId)
        {
            var userRooms = _context.UserRoom.Where(ur => ur.RoomId == roomId).AsNoTracking();
            return userRooms;
        }

        /// <summary>
        /// Возвращает пользователей, принадлежащих к комнате, асинхронно.
        /// </summary>
        /// <param name="roomId">Идентификатор комнаты</param>
        /// <returns>Список пользователей, принадлежащих к комнате</returns>
        public async Task<IEnumerable<ApplicationUser>> GetUsersAsync(Guid roomId)
        {
            var users = _context.UserRoom.Where(r => r.RoomId == roomId).Select(x => x.User).AsNoTracking();
            return await users.ToListAsync();
        }

        public async Task<bool> SaveAsync()
        {
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        /// <summary>
        /// Обновляет запись пользовательской комнаты асинхронно.
        /// </summary>
        /// <param name="updateModel">Модель данных для обновления</param>
        /// <returns>True, если операция выполнена успешно, иначе False</returns>

        public async Task<bool> UpdateAsync(UserRoom updateModel)
        {
            _context.UserRoom.Update(updateModel);
            return await SaveAsync();
        }

        /// <summary>
        /// Возвращает комнаты по идентификатору пользователя асинхронно.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns>Список комнат, к которым принадлежит пользователь</returns>
        public async Task<IEnumerable<Room>> GetRoomsByUserIdAsync(Guid userId)
        {
            var rooms = _context.UserRoom.Where(u => u.UserId == userId).Select(r => r.Room);
            return await rooms.ToListAsync();
        }
        /// <summary>
        /// Удаляет пользователя из комнаты асинхронно.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="roomId">Идентификатор комнаты</param>
        /// <returns>True, если операция выполнена успешно, иначе False</returns>
        public async Task<bool> DeleteUserInRoom(Guid userId, Guid roomId)
        {
            var userRoom = await _context.UserRoom.FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoomId == roomId);
            _context.UserRoom.Remove(userRoom);
            return await SaveAsync(); 
        }

        public async Task<bool> UpdateAsync(List<UserRoom> userRooms)
        {
            _context.UserRoom.UpdateRange(userRooms);
            return await SaveAsync();
        }
    }
}
