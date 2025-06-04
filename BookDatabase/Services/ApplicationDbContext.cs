using BookDatabase.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace BookDatabase.Services
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Book> Books { get; set; }
    }
}
