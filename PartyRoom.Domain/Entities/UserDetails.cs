using System.ComponentModel.DataAnnotations;

namespace PartyRoom.Domain.Entities
{
    public class UserDetails
    {
        [Key]
        public Guid ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string? About { get; set; }
        public string? ImagePath { get; set; } = "https://android-obzor.com/wp-content/uploads/2022/03/a-1-1024x1024.jpeg";
    }
}
