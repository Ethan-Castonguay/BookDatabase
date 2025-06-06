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
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
