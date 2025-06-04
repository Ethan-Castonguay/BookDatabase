using Microsoft.AspNetCore.Mvc;
using BookDatabase.Models;
using BookDatabase.Services;

namespace BookDatabase.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext context;
        public BooksController(ApplicationDbContext context) 
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            var books = context.Books.OrderByDescending(p => p.Id).ToList();
            return View(books);
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}
