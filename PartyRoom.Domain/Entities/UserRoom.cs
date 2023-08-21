namespace PartyRoom.Domain.Entities
{
    public class UserRoom
    {
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
        public Guid RoomId { get; set; }
        public Room Room { get; set; }
        public Guid DestinationUserId { get; set; } = Guid.Empty;
    }
}
