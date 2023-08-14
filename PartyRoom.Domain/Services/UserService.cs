using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PartyRoom.Contracts.DTOs.User;
using PartyRoom.Domain.Entities;
using PartyRoom.Domain.Interfaces.Services;
using System.Security.Claims;

namespace PartyRoom.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        public UserService(IMapper mapper, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public IQueryable<ApplicationUser> Users => _userManager.Users;

        public IQueryable<ApplicationRole> Roles => _roleManager.Roles;

        public async Task CreateRoleAsync(string roleName)
        {
            if (string.IsNullOrEmpty(roleName) )
            {
                throw new ArgumentNullException(nameof(roleName));
            }
            var roleFind = await _roleManager.FindByNameAsync(roleName);

            if (roleFind!=null)
            {
                throw new InvalidOperationException(ExceptionMessages.DuplicateItem);
            }
            var role = new ApplicationRole { Name = roleName };
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(ExceptionMessages.CreationFailed);
            }
        }

        public async Task CreateUserAsync(UserRegistrationDTO createModel)
        {
            if (createModel == null)
            {
                throw new ArgumentNullException(nameof(createModel));
            }
            var userMap = _mapper.Map<ApplicationUser>(createModel);
            if (userMap == null)
            {
                throw new InvalidOperationException(ExceptionMessages.MappingFailed);
            }

            if ((await _userManager.FindByNameAsync(userMap.UserName)) != null)
            {
                throw new InvalidOperationException(ExceptionMessages.DuplicateItem);
            }
            var result = await _userManager.CreateAsync(userMap, createModel.Password);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(ExceptionMessages.CreationFailed);
            }
            var role = await _roleManager.FindByNameAsync("User");
            if (role == null)
            {
                throw new InvalidOperationException(ExceptionMessages.SearchFailed);
            }

            await _userManager.AddToRoleAsync(userMap, role.Name);
            var userId = await _userManager.GetUserIdAsync(userMap);
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("Role", role.Name));
            claims.Add(new Claim("Username", userMap.UserName));
            claims.Add(new Claim("Id", userId));

            await _userManager.AddClaimsAsync(userMap, claims);
        }

        public async Task DeleteRoleAsync(ApplicationRole deleteRole)
        {
            if (deleteRole == null)
            {
                throw new ArgumentNullException(nameof(deleteRole));
            }
            var result = await _roleManager.DeleteAsync(deleteRole);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(ExceptionMessages.DeletionFailed);
            }
        }

        public async Task DeleteUserByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }
            var deletedUser = await _userManager.FindByIdAsync(id.ToString());
            if (deletedUser == null)
            {
                throw new InvalidOperationException(ExceptionMessages.SearchFailed);
            }
            var result = await _userManager.DeleteAsync(deletedUser);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(ExceptionMessages.DeletionFailed);
            }
        }

        public async Task<ApplicationRole> GetRoleByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                throw new InvalidOperationException(ExceptionMessages.SearchFailed);
            }

            return role;
        }

        public async Task<ApplicationRole> GetRoleByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            var role = await _roleManager.FindByNameAsync(name);
            if (role == null)
            {
                throw new InvalidOperationException(ExceptionMessages.SearchFailed);
            }
            return role;
        }

        public async Task<ICollection<string>> GetRolesByUserNameAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                throw new InvalidOperationException(ExceptionMessages.SearchFailed);
            }

            var roles = await _userManager.GetRolesAsync(user);
            return roles;
        }

        public async Task<PublicUserDTO> GetUserByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                throw new InvalidOperationException(ExceptionMessages.SearchFailed);
            }

            var userMap = _mapper.Map<PublicUserDTO>(user);
            if (userMap == null)
            {
                throw new InvalidOperationException(ExceptionMessages.MappingFailed);
            }

            return userMap;
        }

        public async Task<PublicUserDTO> GetUserByNameAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                throw new InvalidOperationException(ExceptionMessages.SearchFailed);
            }

            var userMap = _mapper.Map<PublicUserDTO>(user);
            if (userMap == null)
            {
                throw new InvalidOperationException(ExceptionMessages.MappingFailed);
            }

            return userMap;
        }

        public Task<ICollection<UserDTO>> GetUsersByRoleIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<UserDTO>> GetUsersByRoleNameAsync(string roleName)
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationUser> LoginAsync(UserLoginDTO loginModel)
        {
            if (loginModel == null)
            {
                throw new ArgumentNullException(nameof(loginModel));
            }
            var userFind = await _userManager.FindByEmailAsync(loginModel.Email);
            var result = await _signInManager.PasswordSignInAsync(userFind, loginModel.Password,false,false);
            if(!result.Succeeded) 
            {
                throw new InvalidOperationException(ExceptionMessages.SearchFailed);
            }
            return userFind;
        }

        public async Task UpdateRoleAsync(ApplicationRole updateModel)
        {
            if (updateModel == null)
            {
                throw new ArgumentNullException(nameof(updateModel));
            }
            var result = await _roleManager.UpdateAsync(updateModel);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(ExceptionMessages.UpdateFailed);
            }
        }

        public async Task UpdateUserAsync(PublicUserDTO updateModel)
        {
            if (updateModel == null)
            {
                throw new ArgumentNullException(nameof(updateModel));
            }
            var userMap = _mapper.Map<ApplicationUser>(updateModel);
            if (userMap == null)
            {
                throw new InvalidOperationException(ExceptionMessages.MappingFailed);
            }
            var result = await _userManager.UpdateAsync(userMap);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(ExceptionMessages.UpdateFailed);
            }
        }
    }
}
