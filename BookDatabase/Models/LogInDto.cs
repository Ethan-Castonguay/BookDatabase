using System.ComponentModel.DataAnnotations;

namespace BookDatabase.Models
{
    public class LogInDto
    {
        [EmailAddress]
        public string email { get; set; } = "";
        [Required]
        [MaxLength(30)]
        public string password { get; set; } = "";
    }
}
