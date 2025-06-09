using BookDatabase.Models;
using BookDatabase.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookDatabase.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpDto signUpDto)
        {
            if (!ModelState.IsValid)
            {
                return View(signUpDto);
            }

            if (signUpDto.password != signUpDto.secondAttemptPassword)
            {
                ModelState.AddModelError("Password", "Passwords do not match.");
                return View(signUpDto);
            }

            var user = new IdentityUser
            {
                UserName = signUpDto.email,
                Email = signUpDto.email,
                PhoneNumber = signUpDto.phone
            };

            var result = await _userManager.CreateAsync(user, signUpDto.password);

            //this doesn't happen
            if (result.Succeeded)
            {
                Console.WriteLine("This works 4");
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                //yes (2 times)
                Console.WriteLine("This works 5");
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(signUpDto);
        }

        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LogInDto logInDto)
        {
            if (!ModelState.IsValid)
            {
                return View(logInDto);
            }

            var result = await _signInManager.PasswordSignInAsync(logInDto.email, logInDto.password, false, false);

            if (result.Succeeded)
            {
                Console.WriteLine("AC86");
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(logInDto);
        }

    }
}
