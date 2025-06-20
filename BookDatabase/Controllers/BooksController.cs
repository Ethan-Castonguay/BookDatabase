using BookDatabase.Models;
using BookDatabase.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace BookDatabase.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;
        private readonly UserManager<IdentityUser> userManager;

        public BooksController(ApplicationDbContext context, IWebHostEnvironment environment, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.environment = environment;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index(string selected = "id", string isDown = "true")
        {
            var userId = userManager.GetUserId(User);
            var books = context.Books.Where(b => b.UserId == userId);

            books = (selected.ToLower(), isDown) switch
            {
                ("title", "true") => books.OrderBy(b => b.title),
                ("title", "false") => books.OrderByDescending(b => b.title),
                ("author", "true") => books.OrderBy(b => b.author),
                ("author", "false") => books.OrderByDescending(b => b.author),
                _ => isDown == "false" ? books.OrderBy(b => b.Id) : books.OrderByDescending(b => b.Id)
            };

            var sorter = new TableSorter
            {
                books = books.ToList(),
                selected = selected,
                isDown = isDown
            };

            return View(sorter);
        }

        [HttpGet]
        public async Task<IActionResult> UserStats()
        {
            var userId = userManager.GetUserId(User);
            var books = await context.Books.Where(b => b.UserId == userId).ToListAsync();

            var bookCount = books.Count;
            var ratedBooks = books.Where(b => b.starRating > 0).ToList();
            var avgRating = ratedBooks.Any() ? ratedBooks.Average(b => b.starRating) : 0;
            var favoriteGenre = books
                .GroupBy(b => b.genre)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault() ?? "N/A";

            var html = $@"
            <li><span class='dropdown-item-text'>📚 Books: {bookCount}</span></li>
            <li><span class='dropdown-item-text'>⭐ Avg. Rating: {avgRating:0.0}</span></li>
            <li><span class='dropdown-item-text'>🏷️ Favorite Genre: {favoriteGenre}</span></li>";

            return Content(html, "text/html");
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
           

            //Save the image file
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(bookDto.ImageFile!.FileName);

            string imageFullPath = Path.Combine(environment.WebRootPath, "Images", newFileName);
            //stream is the path to the image, then copy the image file from bookDto at the path
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                bookDto.ImageFile.CopyTo(stream);
            }

            var userId = userManager.GetUserId(User);

            Book book = new Book()
            {
                title = bookDto.title,
                publicationYear = bookDto.publicationYear,
                author = bookDto.author,
                Status = bookDto.Status,
                genre = bookDto.genre,
                Completion = bookDto.Completion,
                chapters = bookDto.chapters,
                starRating = bookDto.starRating,
                review = bookDto.review,
                ImageFileName = newFileName,
                UserId = userId!
            };

            context.Books.Add(book);
            context.SaveChanges();

            return RedirectToAction("Index", "Books");
        }

        public IActionResult Edit(int id)
        {
            var userId = userManager.GetUserId(User);
            //in the Books table, find the match or return null, the book must have the correct id and the users id must match the one logged in right now
            var book = context.Books.FirstOrDefault(b => b.Id == id && b.UserId == userId);

            if (book == null)
            {
                return RedirectToAction("Index", "Books");
            }

            var bookDto = new BookDto()
            {
                title = book.title,
                publicationYear = book.publicationYear,
                author = book.author,
                Status = book.Status,
                genre = book.genre,
                Completion = book.Completion,
                chapters = book.chapters,
                starRating = book.starRating,
                review = book.review
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

            string newFileName = book.ImageFileName!;

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
                string oldImagePath = Path.Combine(imagesFolder, book.ImageFileName!);
                if (System.IO.File.Exists(oldImagePath))
                {
                    try
                    {
                        System.IO.File.SetAttributes(oldImagePath, FileAttributes.Normal); // In case it's read-only
                        System.IO.File.Delete(oldImagePath);
                    }
                    catch (Exception ex)
                    {
                        // Log error or show message, but don't crash
                        Console.WriteLine($"Could not delete old image: {ex.Message}");
                    }
                }
            }


            book.title = bookDto.title;
            book.publicationYear = bookDto.publicationYear;
            book.author = bookDto.author;
            book.ImageFileName = newFileName;
            book.Status = bookDto.Status;
            book.genre = bookDto.genre;
            book.Completion = bookDto.Completion;
            book.chapters = bookDto.chapters;
            book.starRating = bookDto.starRating;
            book.review = bookDto.review;

            context.SaveChanges(true);

            return RedirectToAction("Index", "Books");
        }

        public IActionResult Delete(int id)
        {
            var userId = userManager.GetUserId(User);
            var book = context.Books.FirstOrDefault(b => b.Id == id && b.UserId == userId);

            if (book == null)
            {
                return RedirectToAction("Index", "Books");
            }

            string fullImagePath = Path.Combine(environment.WebRootPath, "Images", book.ImageFileName!);

            if (System.IO.File.Exists(fullImagePath))
            {
                try
                {
                    System.IO.File.SetAttributes(fullImagePath, FileAttributes.Normal);
                    System.IO.File.Delete(fullImagePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Could not delete old image: {ex.Message}");
                }
            }

            context.Books.Remove(book);
            context.SaveChanges(true);

            return RedirectToAction("Index", "Books");
        }
    }
}