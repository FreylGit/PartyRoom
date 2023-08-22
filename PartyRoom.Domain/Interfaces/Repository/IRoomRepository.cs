using PartyRoom.Domain.Entities;

namespace PartyRoom.Domain.Interfaces.Repository
{
    public interface IRoomRepository : IRepositoryBase<Room>
    {
        public IQueryable<Room> Rooms { get; }
        public Task<bool> ExistsLinkAsync(string connectLink);
        public Task<bool> DeleteUserFromRoomAsync(Guid userId, Guid roomId);
        public Task<Room> GetRoomByLinkAsync(string connectionLink);
        public Task<bool> IsAuthorAsync(Guid roomId,Guid userId);
    }
}
