using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookDatabase.Models
{
    public class PfpImg
    {
        public int Id { get; set; }
        public string? ImageFileName { get; set; } = "";
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        // 🔑 Link to IdentityUser
        public string UserId { get; set; } = null!;

        [ForeignKey("UserId")]
        public IdentityUser? User { get; set; }
    }
}
