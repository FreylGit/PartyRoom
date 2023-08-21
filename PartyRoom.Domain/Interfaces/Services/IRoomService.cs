using PartyRoom.Contracts.DTOs.Room;

namespace PartyRoom.Domain.Interfaces.Services
{
    public interface IRoomService
    {
        public Task CreateRoomAsync(RoomCreateDTO createModel, Guid authorId);
        public Task JoinToRoomAsync(string connectLink, Guid userId);
        public Task<RoomInfoDTO> GetRoomInfoAsync(Guid roomId);
        public Task<RoomDto> GetRoomAsync(string link);
        public Task DisconnectUserFromRoom(Guid userId, Guid id);
        public Task DeleteRoomAsync(Guid roomId);
        public Task CheckStartRoomsAsync(CancellationToken stoppingToken);

        public Task CreateTestRoomAsync();
    }
}
