using Microsoft.EntityFrameworkCore;
using PartyRoom.Domain.Entities;
using PartyRoom.Domain.Interfaces.Repository;
using PartyRoom.Infrastructure.Data;

namespace PartyRoom.Infrastructure.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly ApplicationDbContext _context;
        public TokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateAsync(RefreshToken createModel)
        {
            await _context.RefreshTokens.AddAsync(createModel);
            return await SaveAsync();
        }

        public async Task<bool> DeleteAsync(RefreshToken deleteModel)
        {
            _context.RefreshTokens.Remove(deleteModel);
            return await SaveAsync();
        }

        public async Task<bool> ExistsIdAsync(Guid id)
        {
            var refreshToken = await _context.RefreshTokens.AsNoTracking().FirstOrDefaultAsync(rt => rt.ApplicationUserId == id);
            if (refreshToken == null)
            {
                return false;
            }
            return true;
        }

        public Task<bool> ExistsRefreshToken(string token)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<RefreshToken>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<RefreshToken> GetByIdAsync(Guid userId)
        {
            var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.ApplicationUserId == userId);
            return refreshToken;
        }

        public async Task<RefreshToken> GetRefreshToken(string token)
        {
            var refreshToken = await _context.RefreshTokens.AsNoTracking().FirstOrDefaultAsync(rt => rt.Token == token);
            return refreshToken;
        }

        public async Task<bool> SaveAsync()
        {
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> UpdateAsync(RefreshToken updateModel)
        {
            _context.RefreshTokens.Update(updateModel);
            return await SaveAsync();
        }
    }
}
