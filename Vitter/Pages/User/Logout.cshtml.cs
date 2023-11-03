using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Vitter.Utils;

namespace Vitter.Pages.User;

public class Logout : PageModel
{
    private readonly SignInManager _signInManager;

    public Logout(SignInManager signInManager)
    {
        _signInManager = signInManager;
    }

    public IActionResult OnGet()
    {
        _signInManager.SignOut(HttpContext);

        return RedirectToPage("/Index");
    }
}