using PartyRoom.Domain.Interfaces.Repository;
using PartyRoom.Domain.Interfaces.Services;
using PartyRoom.Domain.Services;
using PartyRoom.Infrastructure.Repositories;
using PartyRoom.WebAPI.Services;
using System.Runtime.CompilerServices;

namespace PartyRoom.WebAPI.Extensions
{
    public static class AddServicesExtensions
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IUserRoomRepository, UserRoomRepository>();
        }
        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<JwtService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoomService, RoomService>();
        }
    }
}
