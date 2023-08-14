using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PartyRoom.Domain.Entities;

namespace PartyRoom.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<UserRoom> UserRoom { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            CreateRole(modelBuilder);


            modelBuilder.Entity<Room>()
                .HasOne(r => r.Author)
                .WithMany(u => u.CreatedRooms)
                .HasForeignKey(r => r.AuthorId);


            modelBuilder.Entity<UserRoom>()
                .HasKey(ur => new { ur.UserId, ur.RoomId });

            modelBuilder.Entity<UserRoom>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoom)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserRoom>()
                .HasOne(ur => ur.Room)
                .WithMany(r => r.UserRoom)
                .HasForeignKey(ur => ur.RoomId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        private static void CreateRole(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationRole>().HasData(
            new ApplicationRole { Id = Guid.NewGuid(), Name = "Admin", NormalizedName = "ADMIN" },
            new ApplicationRole { Id = Guid.NewGuid(), Name = "User", NormalizedName = "USER" }
    );
        }
    }
}
