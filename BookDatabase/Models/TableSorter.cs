using System.ComponentModel.DataAnnotations;

namespace BookDatabase.Models
{
    public class TableSorter
    {
        [Required]
        public List<Book> books;
        public string selected { get; set; } = "";
        public string isDown { get; set; } = "true";
    }
} 