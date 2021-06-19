using System.ComponentModel.DataAnnotations;

namespace LightController.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage="Podaj nazwę użytkownika")]
        public string UserName { get; set; }
        [Required(ErrorMessage="Podaj hasło")]
        public string Password { get; set; }
    }
}