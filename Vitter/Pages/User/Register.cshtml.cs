using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Vitter.Database;
using Vitter.Utils;

namespace Vitter.Pages.User;

public class Register : PageModel
{
    public string Error { get; set; }

    private readonly UserRepository _userRepository;
    private readonly SignInManager _signInManager;

    public Register(UserRepository userRepository, SignInManager signInManager)
    {
        _userRepository = userRepository;
        _signInManager = signInManager;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync([FromForm] RegisterForm form)
    {
        if (string.IsNullOrEmpty(form.Username) || string.IsNullOrEmpty(form.Email) ||
            string.IsNullOrEmpty(form.Password))
        {
            Error = "All fields are required";

            return Page();
        }

        var hashedPassword = Hasher.CreateMd5(form.Password);

        var id = await _userRepository.CreateUser(form.Username, hashedPassword, form.Email);

        await _signInManager.SignInUser(id, HttpContext);

        return RedirectToPage("/Index");
    }
}

public class RegisterForm
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}