using System.ComponentModel.DataAnnotations;

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
    }
}
