using PartyRoom.Contracts.DTOs.User;

namespace PartyRoom.Domain.Interfaces.Services
{
    public interface IProfileService
    {
        public Task<UserProfileDTO> GetCurrentProfileAsync(Guid userId);
        public Task<UserPublicDTO> GetPublicUserProfileAsync(Guid userId);
    }
}
