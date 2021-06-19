using System.ComponentModel.DataAnnotations;

namespace LightController.Models
{
    public class PasswordReset2Model
    {
        [Required(ErrorMessage = "Hasło jest wymagane")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Hasła nie są identyczne")]
        public string Password2 { get; set; }
    }
}