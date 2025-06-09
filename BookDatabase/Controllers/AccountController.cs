using Microsoft.AspNetCore.Mvc;
using BookDatabase.Models;
using BookDatabase.Services;
using Microsoft.EntityFrameworkCore;

namespace BookDatabase.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(SignUpDto signUpDto)
        {
            User user = new User()
            {
                firstName = signUpDto.firstName,
                lastName = signUpDto.lastName,
                email = signUpDto.email,
                phone = signUpDto.phone,
                password = signUpDto.password,
            };

            return RedirectToAction("Index", "Home");
        }

        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LogIn(LogInDto logInDto)
        {
            User user = new User()
            {
                email = logInDto.email,
                password = logInDto.password,
            };

            return RedirectToAction("Index", "Home");
        }
    }
}
