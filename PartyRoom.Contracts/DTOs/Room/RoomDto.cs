namespace PartyRoom.Contracts.DTOs.Room
{
    public class RoomDto
    {
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime StartDate { get; set; }
    }
}
