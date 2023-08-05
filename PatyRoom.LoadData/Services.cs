using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PartyRoom.Infrastructure.Data;
using PartyRoom.WebAPI.MappingProfiles.EntityToDto;

namespace PatyRoom.LoadData
{
    public static class Services
    {
        private static string connectionString = "Server=ANDREY;Database=PartyRoom;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public static ApplicationDbContext GetApplicationDbContext()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(connectionString)
                .Options;
            return new ApplicationDbContext(options);
        }
        public static IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<UserMappingProfile>();
            });
            var mapper = config.CreateMapper();
            return mapper;
        }
    }
}
