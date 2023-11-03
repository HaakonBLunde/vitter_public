using System.Security.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Vitter.Utils;

namespace Vitter.Pages.User;

public class Login : PageModel
{
    public bool HasError { get; set; }

    private readonly SignInManager _signInManager;

    public Login(SignInManager signInManager)
    {
        _signInManager = signInManager;
    }

    public void OnGet(bool hasError)
    {
        HasError = hasError;
    }

    public async Task<IActionResult> OnPostAsync([FromForm] SignInForm form)
    {
        if (string.IsNullOrEmpty(form.Username) || string.IsNullOrEmpty(form.Password))
        {
            return RedirectToPage(new { HasError = true });
        }

        try
        {
            var hashedPassword = Hasher.CreateMd5(form.Password);
            await _signInManager.SignInUser(form.Username, hashedPassword, HttpContext);
        }
        catch (InvalidCredentialException)
        {
            return RedirectToPage(new { HasError = true });
        }


        return RedirectToPage("/Index");
    }
}

public class SignInForm
{
    public string Username { get; set; }
    public string Password { get; set; }
}