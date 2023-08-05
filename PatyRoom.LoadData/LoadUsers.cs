using Microsoft.AspNetCore.Identity;
using PartyRoom.Contracts.DTOs.User;
using PartyRoom.Domain.Entities;
using PartyRoom.Domain.Services;
using PartyRoom.Infrastructure.Data;

namespace PatyRoom.LoadData
{
    public class LoadUsers
    {
        private readonly UserService _userService;
        public LoadUsers()
        {
            var dbContext = Services.GetApplicationDbContext();
            ApplicationUserStore userStore = new ApplicationUserStore(dbContext);
            ApplicationRoleStore roleStore = new ApplicationRoleStore(dbContext);
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore, null, null, null, null, null, null, null, null);
            RoleManager<ApplicationRole> roleManager = new RoleManager<ApplicationRole>(roleStore, null, null, null, null);
            //_userService = new UserService(Services.GetMapper(), userManager, roleManager);
        }

        public async Task LoadListUsersAsync()
        {
            List<UserRegistrationDTO> testData = new List<UserRegistrationDTO>
                {
                    new UserRegistrationDTO
                    {
                        FirstName = "John",
                        LastName = "Doe",
                        UserName = "johndoe",
                        Email = "johndoe@example.com",
                        PhoneNumber = "1234567890",
                        DateOfBirth = new DateTime(1990, 5, 15),
                        Password = "Password123",
                        ConfirmPassword = "Password123"
                    },
                    new UserRegistrationDTO
                    {
                        FirstName = "Alice",
                        LastName = "Smith",
                        UserName = "alicesmith",
                        Email = "alice.smith@example.com",
                        PhoneNumber = "9876543210",
                        DateOfBirth = new DateTime(1985, 10, 25),
                        Password = "SecurePwd!456",
                        ConfirmPassword = "SecurePwd!456"
                    },
                    new UserRegistrationDTO
                    {
                        FirstName = "Bob",
                        LastName = "Johnson",
                        UserName = "bobjohnson",
                        Email = "bob.johnson@example.com",
                        PhoneNumber = "4567891230",
                        DateOfBirth = new DateTime(1995, 8, 12),
                        Password = "StrongPwd789",
                        ConfirmPassword = "StrongPwd789"
                    },
                    new UserRegistrationDTO
                    {
                        FirstName = "Emma",
                        LastName = "Wilson",
                        UserName = "emmawilson",
                        Email = "emma.wilson@example.com",
                        PhoneNumber = "9871236540",
                        DateOfBirth = new DateTime(1988, 4, 2),
                        Password = "Password@123",
                        ConfirmPassword = "Password@123"
                    },
                    new UserRegistrationDTO
                    {
                        FirstName = "Michael",
                        LastName = "Brown",
                        UserName = "michaelbrown",
                        Email = "michael.brown@example.com",
                        PhoneNumber = "6547893210",
                        DateOfBirth = new DateTime(1992, 9, 30),
                        Password = "PwdSecure!555",
                        ConfirmPassword = "PwdSecure!555"
                    },
                    new UserRegistrationDTO
                    {
                        FirstName = "Olivia",
                        LastName = "Martinez",
                        UserName = "oliviamartinez",
                        Email = "olivia.martinez@example.com",
                        PhoneNumber = "7894563210",
                        DateOfBirth = new DateTime(1993, 6, 17),
                        Password = "Olivia123Pwd",
                        ConfirmPassword = "Olivia123Pwd"
                    },
                    new UserRegistrationDTO
                    {
                        FirstName = "James",
                        LastName = "Jones",
                        UserName = "jamesjones",
                        Email = "james.jones@example.com",
                        PhoneNumber = "1239876540",
                        DateOfBirth = new DateTime(1987, 11, 8),
                        Password = "JamesPwd987!",
                        ConfirmPassword = "JamesPwd987!"
                    },
                    new UserRegistrationDTO
                    {
                        FirstName = "Sophia",
                        LastName = "Hernandez",
                        UserName = "sophiahernandez",
                        Email = "sophia.hernandez@example.com",
                        PhoneNumber = "3219876540",
                        DateOfBirth = new DateTime(1984, 3, 22),
                        Password = "Pwd123!Sophia",
                        ConfirmPassword = "Pwd123!Sophia"
                    },
                    new UserRegistrationDTO
                    {
                        FirstName = "William",
                        LastName = "Garcia",
                        UserName = "williamgarcia",
                        Email = "william.garcia@example.com",
                        PhoneNumber = "7893216540",
                        DateOfBirth = new DateTime(1998, 2, 5),
                        Password = "WilliamSecurePwd",
                        ConfirmPassword = "WilliamSecurePwd"
                    },
                    new UserRegistrationDTO
                    {
                        FirstName = "Ava",
                        LastName = "Lee",
                        UserName = "avalee",
                        Email = "ava.lee@example.com",
                        PhoneNumber = "6541239870",
                        DateOfBirth = new DateTime(1991, 7, 11),
                        Password = "PwdAvaLee@111",
                        ConfirmPassword = "PwdAvaLee@111"
                    }
                };
            foreach (var user in testData)
            {
                try
                {
                    await _userService.CreateUserAsync(user);

                }
                catch
                {
                    await Console.Out.WriteLineAsync($"ошибка с загрузкой пользователя{user.Email}");
                }
            }
        }
        public async Task LoadListRoleAsync()
        {
            List<ApplicationRole> rolesList = new List<ApplicationRole>
            {
                new ApplicationRole { Name = "Admin", NormalizedName = "ADMIN" },
                new ApplicationRole { Name = "Moderator", NormalizedName = "MODERATOR" },
                new ApplicationRole { Name = "User", NormalizedName = "USER" },
            };
            foreach (var role in rolesList)
            {
                await _userService.CreateRoleAsync(role);
            }
        }

        public async Task PrintRolles()
        {
            var roles = _userService.Roles;
            foreach (var role in roles)
            {
                await Console.Out.WriteLineAsync(role.Name + " " + role.Id);
            }
        }
        public async Task PrintRole()
        {
            Guid id = Guid.Parse("d6ff5b63-a0b8-41eb-6b73-08db905466b9");
            var role = await _userService.GetRoleByIdAsync(id);
            await Console.Out.WriteLineAsync(role.NormalizedName);
        }
        public async Task PrintUser()
        {
            var user = _userService.Users.FirstOrDefault();
            var userfind = await _userService.GetUserByIdAsync(user.Id);
            await Console.Out.WriteLineAsync(userfind.UserName);
        }
    }
}
