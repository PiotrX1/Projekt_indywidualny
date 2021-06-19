using System.ComponentModel.DataAnnotations;

namespace LightController.Models
{
    public class PasswordResetModel
    {
        [Required(ErrorMessage = "Adres email jest wymagany")]
        [RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-‌​]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Email jest nieprawidłowy")]
        public string Email { get; set; }
    }
}