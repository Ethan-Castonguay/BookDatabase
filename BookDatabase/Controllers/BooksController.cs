using Microsoft.AspNetCore.Mvc;
using BookDatabase.Models;
using BookDatabase.Services;

namespace BookDatabase.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;

        public BooksController(ApplicationDbContext context, IWebHostEnvironment environment) 
        {
            this.context = context;
            this.environment = environment;
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

        [HttpPost]
        public IActionResult Create(BookDto bookDto)
        {
            if (bookDto.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "The image file is required");
            }

            if (!ModelState.IsValid)
            {
                return View(bookDto);
            }

            //Save the image file (revise this)
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(bookDto.ImageFile!.FileName);

            string imageFullPath = environment.WebRootPath + "/Images/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                bookDto.ImageFile.CopyTo(stream);
            }

            Book book = new Book()
            {
                title = bookDto.title,
                publicationYear = bookDto.publicationYear,
                author = bookDto.author,
                ImageFileName = newFileName,

            };

            context.Books.Add(book);
            context.SaveChanges();

            return RedirectToAction("Index", "Books");
        }
    }
}