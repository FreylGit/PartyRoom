using PartyRoom.Domain.Entities;

namespace PartyRoom.Domain.Interfaces.Repository
{
    public interface IUserRepository : IRepositoryBase<ApplicationUser>
    {
        public Task<ApplicationUser> GetProfileAsync(Guid userId);
    }
}
