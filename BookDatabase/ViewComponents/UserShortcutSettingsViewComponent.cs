using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using BookDatabase.Services; // Replace with your actual DbContext namespace
using BookDatabase.Models; // Replace with your models namespace
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BookDatabase.ViewComponents 
{ 
public class UserShortcutSettingsViewComponent : ViewComponent
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public UserShortcutSettingsViewComponent(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user == null)
            return View(new UserSettingsDto()); // return empty/default if not logged in

        var settings = await _context.userSettings.FirstOrDefaultAsync(s => s.UserId == user.Id);

        var dto = settings == null
            ? new UserSettingsDto()
            : new UserSettingsDto
            {
                homeShortcut = settings.homeShortcut,
                booksShortcut = settings.booksShortcut,
                aboutShortcut = settings.aboutShortcut,
                privacyShortcut = settings.privacyShortcut,
                settingsShortcut = settings.settingsShortcut,
                darkModeShortcut = settings.darkModeShortcut,
                createBookShortcut = settings.createBookShortcut,
                searchbarFocusShortcut = settings.searchbarFocusShortcut,
                genreFilterShortcut = settings.genreFilterShortcut,
                ownershipFilterShortcut = settings.ownershipFilterShortcut
            };

        return View(dto);
    }
}
}