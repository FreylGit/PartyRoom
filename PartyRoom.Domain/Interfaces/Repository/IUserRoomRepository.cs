using PartyRoom.Domain.Entities;

namespace PartyRoom.Domain.Interfaces.Repository
{
    public interface IUserRoomRepository : IRepositoryBase<UserRoom>
    {
        public IQueryable<UserRoom> UserRooms { get; }
        public Task<bool> DeleteUserRoomsAsync(IQueryable<UserRoom> userRooms);
        public IQueryable<UserRoom> GetUserRooms(Guid roomId);
    }
}
