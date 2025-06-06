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

            string imagesFolder = Path.Combine(environment.WebRootPath, "Images");
            if (!Directory.Exists(imagesFolder))
            {
                Directory.CreateDirectory(imagesFolder);
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
                string imagesFolder = Path.Combine(environment.WebRootPath, "Images");

                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                }

                // Save new image
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(bookDto.ImageFile.FileName);
                string imageFullPath = Path.Combine(imagesFolder, newFileName);

                using (var stream = new FileStream(imageFullPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    bookDto.ImageFile.CopyTo(stream);
                }

                // Safely delete old image
                string oldImagePath = Path.Combine(imagesFolder, book.ImageFileName);
                if (System.IO.File.Exists(oldImagePath))
                {
                    try
                    {
                        System.IO.File.SetAttributes(oldImagePath, FileAttributes.Normal); // In case it's read-only
                        System.IO.File.Delete(oldImagePath);
                    }
                    catch (Exception ex)
                    {
                        // Optional: log error or show message, but don't crash
                        Console.WriteLine($"Could not delete old image: {ex.Message}");
                    }
                }
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

            if (System.IO.File.Exists(fullImagePath))
            {
                try
                {
                    System.IO.File.SetAttributes(fullImagePath, FileAttributes.Normal); // In case it's read-only
                    System.IO.File.Delete(fullImagePath);
                    
                }
                catch (Exception ex)
                {
                    // Optional: log error or show message, but don't crash
                    Console.WriteLine($"Could not delete old image: {ex.Message}");
                }
            }
            context.Books.Remove(book);
            context.SaveChanges(true);
            return RedirectToAction("Index", "Books");
        }

    }
}