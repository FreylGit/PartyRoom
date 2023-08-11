﻿using PartyRoom.Contracts.DTOs.Room;
using PartyRoom.Contracts.DTOs.User;

namespace PartyRoom.Domain.Interfaces.Services
{
    public interface IRoomService
    {
        public Task CreateRoomAsync(RoomCreateDTO createModel, Guid authorId);
        public Task JoinToRoomAsync(string connectLink, Guid userId);
        public Task<IEnumerable<PublicUserDTO>> GetUsersByRoomAsync(Guid roomId);
        public Task<IEnumerable<RoomDto>> GetRoomsByUserAsync(Guid userId);
        public Task DeleteUserFromRoomAsync(Guid userId, Guid id);
        public Task DeleteRoomAsync(Guid roomId);
    }
}