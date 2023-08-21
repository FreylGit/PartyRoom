namespace PartyRoom.Contracts.DTOs.Room
{
    public class RoomCreateDTO
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
    }
}
