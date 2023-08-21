using PartyRoom.Domain.Entities;

namespace PartyRoom.Domain.Interfaces.Services
{
    public interface IAccountService
    {
        public Task<RefreshToken> GetRefreshTokenAsync(string token);
        public Task UpdateRefreshTokenAsync(RefreshToken refreshToken);
        public Task CreateRefreshTokenAsync(RefreshToken refreshToken);
    }
}
