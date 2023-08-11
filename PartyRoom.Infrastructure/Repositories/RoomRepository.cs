using Microsoft.EntityFrameworkCore;
using PartyRoom.Domain.Entities;
using PartyRoom.Domain.Interfaces.Repository;
using PartyRoom.Infrastructure.Data;

namespace PartyRoom.Infrastructure.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly SemaphoreSlim _contextLock = new SemaphoreSlim(1);
        public RoomRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IQueryable<Room> Rooms => _context.Rooms;

        public async Task<bool> CreateAsync(Room createModel)
        {
            await _context.Rooms.AddAsync(createModel);
            var userRoom = new UserRoom { Room = createModel, UserId = createModel.AuthorId };
            await _context.UserRoom.AddAsync(userRoom);
            return await SaveAsync();
        }

        public async Task<bool> DeleteAsync(Room deleteModel)
        {
            _context.Rooms.Remove(deleteModel);
            return await SaveAsync();
        }

        public async Task<bool> DeleteUserFromRoomAsync(Guid userId, Guid roomId)
        {
            var userRoom = await _context.UserRoom.FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoomId == roomId);
            _context.UserRoom.Remove(userRoom);
            return await SaveAsync();
        }

        public async Task<bool> DeleteUserRoomsAsync(IQueryable<UserRoom> userRooms)
        {
            _context.UserRoom.RemoveRange(userRooms);
            return await SaveAsync();
        }

        public async Task<bool> ExistsIdAsync(Guid id)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(room => room.Id == id);
            if (room == null)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> ExistsLinkAsync(string connectLink)
        {
            if (await _context.Rooms.AnyAsync(r => r.Link == connectLink))
            {
                return true;
            }
            return false;
        }

        public async Task<bool> ExistsUserInRoomAsync(Guid userId)
        {
            if (await _context.UserRoom.AnyAsync(u => u.UserId == userId))
            {
                return true;
            }
            return false;
        }

        public async Task<IQueryable<Room>> GetAllAsync()
        {
            return await Task.FromResult(_context.Rooms.AsQueryable());
        }

        public async Task<Room> GetByIdAsync(Guid id)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id);
            return room;
        }

        public async Task<Room> GetRoomByLinkAsync(string connectionLink)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Link == connectionLink);
            return room;
        }

        public async Task<IEnumerable<Room>> GetRoomsByUserIdAsync(Guid userId)
        {
            var rooms = _context.UserRoom.Where(u => u.UserId == userId).Select(r => r.Room);
            return await rooms.ToListAsync();
        }

        public IQueryable<UserRoom> GetUserRoomsById(Guid roomId)
        {
            var userRooms = _context.UserRoom.Where(ur => ur.RoomId == roomId);
            return userRooms;
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersByRoomAsync(Guid roomId)
        {
            var users = _context.UserRoom.Where(r => r.RoomId == roomId).Select(x => x.User);

            return await users.ToListAsync();
        }

        public async Task<bool> SaveAsync()
        {
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> SlugExistsAsync(string slug)
        {
            bool exists = await _context.Rooms.AnyAsync(r => r.Link == slug);
            return exists;
        }

        public async Task<bool> UpdateAsync(Room updateModel)
        {
            _context.Rooms.Update(updateModel);
            return await SaveAsync();
        }
    }
}
