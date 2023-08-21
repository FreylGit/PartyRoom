using PartyRoom.Contracts.DTOs.User;

namespace PartyRoom.Domain.Interfaces.Services
{
    public interface IProfileService
    {
        public Task<UserProfileDTO> GetCurrentProfile(Guid userId);
        public Task<PublicUserDTO> GetPublicUserProfile(Guid userId);
    }
}
