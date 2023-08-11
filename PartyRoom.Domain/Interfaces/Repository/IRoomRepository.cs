using PartyRoom.Domain.Entities;

namespace PartyRoom.Domain.Interfaces.Repository
{
    public interface IRoomRepository : IRepositoryBase<Room>
    {
        public IQueryable<Room> Rooms { get; }
        public Task<bool> SlugExistsAsync(string slug);
        public Task<bool> ExistsLinkAsync(string connectLink);
        public Task<IEnumerable<ApplicationUser>> GetUsersByRoomAsync(Guid roomId);
        public Task<bool> ExistsUserInRoomAsync(Guid userId);
        public Task<IEnumerable<Room>> GetRoomsByUserIdAsync(Guid userId);
        public Task<bool> DeleteUserFromRoomAsync(Guid userId, Guid roomId);
        public IQueryable<UserRoom> GetUserRoomsById(Guid roomId);
        public Task<bool> DeleteUserRoomsAsync(IQueryable<UserRoom> userRooms);
        public Task<Room> GetRoomByLinkAsync(string connectionLink);
    }
}
