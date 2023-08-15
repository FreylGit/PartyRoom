using Microsoft.EntityFrameworkCore;
using PartyRoom.Infrastructure.Data;


string connectionString = "Server=ANDREY;Database=PartyRoom;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(connectionString) // Используйте другой провайдер, если не SQL Server
                .Options;
var  context = new ApplicationDbContext(options);
