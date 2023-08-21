using PartyRoom.Domain.Entities;

namespace PartyRoom.Domain.Interfaces.Repository
{
    public interface ITokenRepository : IRepositoryBase<RefreshToken>
    {
        public Task<bool> ExistsRefreshToken(string token);
        public Task<RefreshToken> GetRefreshToken(string token);
    }
}
