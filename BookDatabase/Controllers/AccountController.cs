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
                UserName = (signUpDto.firstName + signUpDto.lastName),
                Email = signUpDto.email,
                PhoneNumber = signUpDto.phone
            };

            var result = await _userManager.CreateAsync(user, signUpDto.password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            } else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(signUpDto);
            }
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

            var user = await _userManager.FindByEmailAsync(logInDto.email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "No account found with this email.");
                return View(logInDto);
            }

            //(username, password, isPersistent(put true if you have a 'remember me' checkbox), lockoutOnFailure(put true if you want users to be punished for failed login attempts)
            var result = await _signInManager.PasswordSignInAsync(user.UserName!, logInDto.password, false, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(logInDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
