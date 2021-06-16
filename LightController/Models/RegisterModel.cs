using System.ComponentModel.DataAnnotations;

namespace LightController.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Nazwa użytkownika jest wymagana")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Adres email jest wymagany")]
        [RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-‌​]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Email jest nieprawidłowy")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Hasło jest wymagane")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Hasła nie są identyczne")]
        public string Password2 { get; set; }
    }
}