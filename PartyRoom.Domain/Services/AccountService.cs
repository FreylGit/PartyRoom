using PartyRoom.Domain.Entities;
using PartyRoom.Domain.Interfaces.Services;
using PartyRoom.Domain.Interfaces.Repository;
namespace PartyRoom.Domain.Services
{
    public class AccountService : IAccountService
    {
        private readonly ITokenRepository _tokenRepository;
        public AccountService(ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }
        public async Task CreateRefreshTokenAsync(RefreshToken refreshToken)
        {
            if(await _tokenRepository.ExistsIdAsync(refreshToken.ApplicationUserId))
            {
               await _tokenRepository.UpdateAsync(refreshToken);
            }
            else
            {
                await _tokenRepository.CreateAsync(refreshToken);
            }
            
        }

        public async Task<RefreshToken> GetRefreshTokenAsync(string token)
        {
            var refreshToken = await _tokenRepository.GetRefreshToken(token);
            return refreshToken;
        }

        public async Task UpdateRefreshTokenAsync(RefreshToken refreshToken)
        {
            await _tokenRepository.UpdateAsync(refreshToken);
        }
    }
}
