using System.ComponentModel.DataAnnotations;

namespace PartyRoom.Contracts.DTOs.User
{
    public class UserLoginDTO
    {
        [Required(ErrorMessage = "Поле 'Email' обязательно для заполнения.")]
        [EmailAddress(ErrorMessage = "Некорректный формат электронной почты.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Поле 'Password' обязательно для заполнения.")]
        [MinLength(6, ErrorMessage = "Минимальная длина пароля должна быть не менее 6 символов.")]
        public string Password { get; set; }
    }
}
