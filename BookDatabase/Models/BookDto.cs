using System.ComponentModel.DataAnnotations;

namespace BookDatabase.Models
{
    public class BookDto
    {
        [Required, MaxLength(100)]
        public string title { get; set; } = "";
        [Required]
        public int publicationYear { get; set; }
        [Required, MaxLength(100)]
        public string author { get; set; } = "";
        public string Status { get; set; } = "Unentered";
        public string genre { get; set; } = "";
        public string Completion { get; set; } = "Want to read";
        public int starRating { get; set; }
        public string review { get; set; } = "";
        public IFormFile? ImageFile { get; set; }
    }
}
