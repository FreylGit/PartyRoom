namespace PartyRoom.Contracts.DTOs.User
{
    public class UserProfileDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? About { get; set; }
        public string? ImagePath { get; set; }
    }
}
