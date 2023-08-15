using PartyRoom.Domain.Entities;

namespace PartyRoom.Domain.Interfaces.Repository
{
    public interface IUserRoomRepository : IRepositoryBase<UserRoom>
    {
        public IQueryable<UserRoom> UserRooms { get; }
        public Task<bool> DeleteUserRoomsAsync(IQueryable<UserRoom> userRooms);
        public Task<bool> DeleteUserInRoom(Guid userId, Guid roomId);
        public IQueryable<UserRoom> GetUserRooms(Guid roomId);
        public Task<bool> ExistsUserInRoomAsync(Guid userId,Guid roomId);
        public Task<IEnumerable<ApplicationUser>> GetUsersAsync(Guid roomId);
        public Task<IEnumerable<Room>> GetRoomsByUserIdAsync(Guid userId);
        public Task<bool> UpdateAsync(List<UserRoom> userRooms);
    }
}
