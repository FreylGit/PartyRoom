namespace PartyRoom.Contracts.DTOs.Room
{
    public class RoomInfoDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
        public bool IsStarted { get; set; } = false;
        public int QuantityUsers { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
    }
}
