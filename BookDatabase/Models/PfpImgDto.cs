using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookDatabase.Models
{
    public class PfpImgDto
    {
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
