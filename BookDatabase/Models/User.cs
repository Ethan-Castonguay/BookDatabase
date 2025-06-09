using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookDatabase.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string firstName { get; set; } = "";
        [Required]
        [MaxLength(100)]
        public string lastName { get; set; } = "";
        [Required]
        [MaxLength(100)]
        public string email { get; set; } = "";
        public string phone { get; set; } = "";
        [Required]
        [MinLength(8)]
        [MaxLength(30)]
        public string password { get; set; } = "";
        public Boolean loggedIn { get; set; }
        
    }
}
