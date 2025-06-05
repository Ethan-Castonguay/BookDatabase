using Microsoft.AspNetCore.Mvc;
using BookDatabase.Models;
using BookDatabase.Services;
using Microsoft.EntityFrameworkCore;

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

            string imageFullPath = Path.Combine(environment.WebRootPath, "Images", newFileName);
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

        public IActionResult Edit(int id)
        {
            var book = context.Books.Find(id);

            if (book == null)
            {
                return RedirectToAction("Index", "Books");
            }

            var bookDto = new BookDto()
            {
                title = book.title,
                publicationYear = book.publicationYear,
                author = book.author,

            };

            ViewData["BookId"] = book.Id;
            ViewData["ImageFileName"] = book.ImageFileName;

            return View(bookDto);
        }

        [HttpPost]
        public IActionResult Edit(int id, BookDto bookDto)
        {
            var book = context.Books.Find(id);

            if (book == null)
            {
                return RedirectToAction("Index", "Books");
            }

            if (!ModelState.IsValid)
            {
                ViewData["BookId"] = book.Id;
                ViewData["ImageFileName"] = book.ImageFileName;

                return View(bookDto);
            }

            string newFileName = book.ImageFileName;
            if (bookDto.ImageFile != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(bookDto.ImageFile!.FileName);

                string imageFullPath = Path.Combine(environment.WebRootPath, "Images", newFileName);
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    bookDto.ImageFile.CopyTo(stream);
                }

                string oldImagePath = Path.Combine(environment.WebRootPath, "Images", book.ImageFileName);

                System.IO.File.Delete(oldImagePath);
            }

            book.title = bookDto.title;
            book.publicationYear = bookDto.publicationYear;
            book.author = bookDto.author;
            book.ImageFileName = newFileName;

            context.SaveChanges(true);

            return RedirectToAction("Index", "Books");
        }

        public IActionResult Delete(int id)
        {
            var book = context.Books.Find(id);

            if (book == null)
            {
                return RedirectToAction("Index", "Books");
            }

            string fullImagePath = Path.Combine(environment.WebRootPath, "Images", book.ImageFileName);

            System.IO.File.Delete(fullImagePath);
            context.Books.Remove(book);
            context.SaveChanges(true);
            return RedirectToAction("Index", "Books");
        }

    }
}