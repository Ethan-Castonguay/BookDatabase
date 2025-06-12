using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookDatabase.Models
{
    public class Book
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string title { get; set; } = "";

        public int publicationYear { get; set; }

        [MaxLength(100)]
        public string author { get; set; } = "";

        public string? ImageFileName { get; set; } = "";

        public string Status { get; set; } = "Unentered";

        public string genre { get; set; } = "";

        public string Completion { get; set; } = "Want to read";

        public int starRating { get; set; }

        public string review { get; set; } = "";

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        // 🔑 Link to IdentityUser
        public string UserId { get; set; } = null!;

        [ForeignKey("UserId")]
        public IdentityUser? User { get; set; }
    }
}

