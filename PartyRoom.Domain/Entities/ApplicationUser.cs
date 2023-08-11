using Microsoft.AspNetCore.Identity;

namespace PartyRoom.Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateRegistration { get; set; }
        public ICollection<Room> CreatedRooms { get; set; }
        public ICollection<UserRoom> UserRoom { get; set; } 
    }
}
