namespace PartyRoom.Contracts.DTOs.Room
{
    public class RoomDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
        public bool IsStarted { get; set; } = false;
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
    }
}
