using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PartyRoom.Domain.Entities;
using PartyRoom.Domain.Interfaces.Repository;
using PartyRoom.Infrastructure.Data;

namespace PartyRoom.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public Task<bool> CreateAsync(ApplicationUser createModel)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(ApplicationUser deleteModel)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<ApplicationUser>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationUser> GetByIdAsync(Guid id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u=> u.Id == id);
            return user;
        }

        public async Task<ApplicationUser> GetProfileAsync(Guid userId)
        {
            var user = await _context.Users.Where(u => u.Id == userId)
                                           .Include(u=>u.UserDetails)
                                           .FirstOrDefaultAsync();
            return user;
        }

        public async Task<bool> SaveAsync()
        {
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> UpdateAsync(ApplicationUser updateModel)
        {
            _context.Users.Update(updateModel); // TODO: Нужно проверить обновляется ли UserDetails
            return await SaveAsync();
        }
    }
}
