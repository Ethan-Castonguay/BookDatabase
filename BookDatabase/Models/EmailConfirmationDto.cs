using System.ComponentModel.DataAnnotations;

namespace BookDatabase.Models
{
    public class EmailConfirmationDto
    {
        [Required]
        [EmailAddress]
        public string email { get; set; } = "";
    }
}
