using PartyRoom.Contracts.DTOs.User;
using PartyRoom.Domain.Entities;

namespace PartyRoom.Domain.Interfaces.Services
{
    public interface IUserService
    {
        #region User
        public IQueryable<ApplicationUser> Users { get; }
        public Task CreateUserAsync(UserRegistrationDTO createModel);
        public Task UpdateUserAsync(PublicUserDTO updateModel);
        public Task DeleteUserByIdAsync(Guid id);
        public Task<PublicUserDTO> GetUserByIdAsync(Guid id);
        public Task<PublicUserDTO> GetUserByNameAsync(string username);
        public Task<ICollection<UserDTO>> GetUsersByRoleNameAsync(string roleName);
        public Task<ICollection<UserDTO>> GetUsersByRoleIdAsync(Guid id);
        public Task<ApplicationUser> LoginAsync(UserLoginDTO loginModel);
        public Task CreateTestUsers();
        #endregion
        #region Role
        public IQueryable<ApplicationRole> Roles { get; }
        public Task CreateRoleAsync(string roleName);
        public Task UpdateRoleAsync(ApplicationRole updateModel);
        public Task DeleteRoleAsync(ApplicationRole deleteModel);
        public Task<ApplicationRole> GetRoleByIdAsync(Guid id);
        public Task<ApplicationRole> GetRoleByNameAsync(string name);
        public Task<ICollection<string>> GetRolesByUserNameAsync(string userName);
        #endregion
    }
}
