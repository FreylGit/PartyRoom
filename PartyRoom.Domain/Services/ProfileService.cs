using AutoMapper;
using PartyRoom.Contracts.DTOs.User;
using PartyRoom.Domain.Interfaces.Repository;
using PartyRoom.Domain.Interfaces.Services;

namespace PartyRoom.Domain.Services
{
    public class ProfileService : IProfileService
    {
        private IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public ProfileService(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public async Task<UserProfileDTO> GetCurrentProfile(Guid userId)
        {
            var userProfile = await _userRepository.GetProfileAsync(userId);
            var userProfileMap = _mapper.Map<UserProfileDTO>(userProfile);
            return userProfileMap;
        }

        public Task<PublicUserDTO> GetPublicUserProfile(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
