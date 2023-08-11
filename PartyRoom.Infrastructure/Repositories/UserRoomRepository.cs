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
        public IQueryable<UserRoom> UserRooms => _context.UserRoom;

        public async Task<bool> CreateAsync(UserRoom createModel)
        {
            await _context.UserRoom.AddAsync(createModel);
            return await SaveAsync();
        }

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

        public async Task<bool> ExistsIdAsync(Guid id)
        {
            var result = await _context.UserRoom.AnyAsync(ur => ur.UserId == id);
            return result;
        }

        public async Task<IQueryable<UserRoom>> GetAllAsync()
        {
            return await Task.FromResult(_context.UserRoom.AsQueryable());
        }

        public async Task<UserRoom> GetByIdAsync(Guid id)
        {
            var userRoom = await _context.UserRoom.FirstOrDefaultAsync(ur => ur.UserId == id);
            return userRoom;
        }

        public IQueryable<UserRoom> GetUserRooms(Guid roomId)
        {
            var userRooms = _context.UserRoom.Where(ur => ur.RoomId == roomId);
            return userRooms;
        }

        public async Task<bool> SaveAsync()
        {
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> UpdateAsync(UserRoom updateModel)
        {
            _context.UserRoom.Update(updateModel);
            return await SaveAsync();
        }
    }
}
