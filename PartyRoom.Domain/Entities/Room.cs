namespace PartyRoom.Domain.Entities
{
    public class Room
    {
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }  
        public ApplicationUser Author { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Link { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public ICollection<UserRoom> UserRoom { get; set; } 
    }
}
