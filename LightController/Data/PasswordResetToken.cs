using System;

namespace LightController.Data
{
    public class PasswordResetToken
    {
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime ExpirationTime { get; set; }
        public bool Used { get; set; }
    }
}