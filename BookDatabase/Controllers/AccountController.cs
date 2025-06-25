using BookDatabase.Models;
using BookDatabase.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net.Mail;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookDatabase.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IWebHostEnvironment environment;
        private readonly IEmailSender _emailSender; 

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IEmailSender emailSender, IWebHostEnvironment environment, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            this.environment = environment;
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var pfp = context.pfpImgs.FirstOrDefault(p => p.UserId == user.Id);
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

        public async Task<IActionResult> SignUp()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var pfp = context.pfpImgs.FirstOrDefault(p => p.UserId == user.Id);
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

                var receiver = signUpDto.email;
                var subject = "Test";
                var message = "To successfully create your account click here";

                await _emailSender.SendEmailAsync(receiver, subject, message);

                return RedirectToAction("Index", "Home");
            } 
            else
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

        public IActionResult EmailConfirmation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EmailConfirmation(EmailConfirmationDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "No account found with this email.");
                return View(model);
            }

            return RedirectToAction("PasswordReset", "Account", new {username = user.UserName});
        }

        public IActionResult PasswordReset(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                RedirectToAction("EmailConfirmation", "Account");
            }
            return View(new PasswordResetDto { Email = username });
        }

        [HttpPost]
        public async Task<IActionResult> PasswordReset(PasswordResetDto model)
            //This currently allows you to reset the password only if you give an email already in the system, don't have to confirm that it belongs to you. This is less safe but way simpler
        {
            if (ModelState.IsValid)
            {
                if (model.newPassword != model.secondAttemptNewPassword)
                {
                    ModelState.AddModelError("Password", "Passwords do not match.");
                    return View(model);
                }

                Console.WriteLine(model.Email);
                var user = await _userManager.FindByNameAsync(model.Email);

                if (user != null)
                {
                    var result = await _userManager.RemovePasswordAsync(user);
                    if (result.Succeeded)
                    {
                        result = await _userManager.AddPasswordAsync(user, model.newPassword);
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email not found");
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong try again");
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemovePfp()
        {
            var userId = _userManager.GetUserId(User);
            var pfp = context.pfpImgs.FirstOrDefault(p => p.UserId == userId);


            if (pfp == null)
            {
                return RedirectToAction("Index", "Home");
            }

            string fullImagePath = Path.Combine(environment.WebRootPath, "Images", Path.GetFileName(pfp.ImageFileName));


            Console.WriteLine(environment.WebRootPath);
            Console.WriteLine(pfp.ImageFileName);
            Console.WriteLine(fullImagePath);

            if (System.IO.File.Exists(fullImagePath))
            {
                Console.WriteLine("2");
                try
                {
                    Console.WriteLine("3");
                    System.IO.File.SetAttributes(fullImagePath, FileAttributes.Normal);
                    System.IO.File.Delete(fullImagePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("4");
                    Console.WriteLine($"Could not delete old image: {ex.Message}");
                }
            }

            context.pfpImgs.Remove(pfp);
            context.SaveChanges(true);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult PfpImgChange()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PfpImgChange(PfpImgDto model)
        {
            if (model.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "The image file is required");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string imagesFolder = Path.Combine(environment.WebRootPath, "Images");
            if (!Directory.Exists(imagesFolder))
            {
                Directory.CreateDirectory(imagesFolder);
            }


            //Save the image file
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(model.ImageFile!.FileName);

            string imageFullPath = Path.Combine(environment.WebRootPath, "Images", newFileName);
            //stream is the path to the image, then copy the image file from bookDto at the path
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                model.ImageFile.CopyTo(stream);
            }

            var userId = _userManager.GetUserId(User);

            PfpImg pfpImg = new PfpImg()
            {
                ImageFileName = "/Images/" + newFileName,
                UserId = userId!
            };

            context.pfpImgs.Add(pfpImg);
            context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

    }
}