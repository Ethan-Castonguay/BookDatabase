using System.ComponentModel.DataAnnotations;

namespace BookDatabase.Models
{
    public class PasswordResetDto
    {
        public string Email { get; set; } = "";
        public string newPassword { get; set; } = "";
        [Required]
        [MaxLength(30)]
        [MinLength(6)]
        public string secondAttemptNewPassword { get; set; } = "";
    }
}
