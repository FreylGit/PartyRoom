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
        public async Task<UserProfileDTO> GetCurrentProfileAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userId));
            }
            var userProfile = await _userRepository.GetProfileAsync(userId);
            if (userProfile == null)
            {
                throw new InvalidOperationException(ExceptionMessages.SearchFailed);
            }
            var userProfileMap = _mapper.Map<UserProfileDTO>(userProfile);
            if(userProfileMap == null)
            {
                throw new InvalidOperationException(ExceptionMessages.MappingFailed);
            }
            return userProfileMap;
        }

        public async Task<UserPublicDTO> GetPublicUserProfileAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userId));
            }
            var userProfile = await _userRepository.GetProfileAsync(userId);

            if (userProfile == null)
            {
                throw new InvalidOperationException(ExceptionMessages.SearchFailed);
            }
            var userProfileMap = _mapper.Map<UserPublicDTO>(userProfile);

            if (userProfileMap == null)
            {
                throw new InvalidOperationException(ExceptionMessages.MappingFailed);
            }
            return userProfileMap;
        }
    }
}
