using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;

namespace BookDatabase.Models
{
    public class UserStats
    {
        public int numBooks { get; set; }
        public string favGenre { get; set; } = "";
        public int avgRating { get; set; }
    }
}