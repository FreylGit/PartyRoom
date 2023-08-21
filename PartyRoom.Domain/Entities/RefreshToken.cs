using System.ComponentModel.DataAnnotations;

namespace PartyRoom.Domain.Entities
{
    public class RefreshToken
    {
        [Key]
        public Guid ApplicationUserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Expires { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
