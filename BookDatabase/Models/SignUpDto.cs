using System.ComponentModel.DataAnnotations;

namespace BookDatabase.Models
{
    public class SignUpDto
    {
        [Required]
        [MaxLength(100)]
        public string firstName { get; set; } = "";
        [Required]
        [MaxLength(100)]
        public string lastName { get; set; } = "";
        [Required]
        public string email { get; set; } = "";
        public string phone { get; set; } = "";
        [Required]
        [MaxLength(30)]
        public string password { get; set; } = "";
        [Required]
        [MaxLength(30)]
        public string secondAttemptPassword { get; set; } = "";
    }
}
