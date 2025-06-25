using BookDatabase.Models;
using BookDatabase.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BookDatabase.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var pfp = _context.pfpImgs.FirstOrDefault(p => p.UserId == user.Id);
                if (pfp != null && pfp.ImageFileName != null)
                {
                    ViewBag.ProfileImgPath = pfp.ImageFileName;
                }
                else
                {
                    ViewBag.ProfileImgPath = Url.Content("~/Images/AnonymousProfilePicture-modified.png");
                }

            }

            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var pfp = _context.pfpImgs.FirstOrDefault(p => p.UserId == user.Id);
                if (pfp != null && pfp.ImageFileName != null)
                {
                    ViewBag.ProfileImgPath = pfp.ImageFileName;
                }
                else
                {
                    ViewBag.ProfileImgPath = Url.Content("~/Images/AnonymousProfilePicture-modified.png");
                }

            }

            return View();
        }
        public async Task<IActionResult> About()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var pfp = _context.pfpImgs.FirstOrDefault(p => p.UserId == user.Id);
                if (pfp != null && pfp.ImageFileName != null)
                {
                    ViewBag.ProfileImgPath = pfp.ImageFileName;
                }
                else
                {
                    ViewBag.ProfileImgPath = Url.Content("~/Images/AnonymousProfilePicture-modified.png");
                }

            }

            return View();
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
