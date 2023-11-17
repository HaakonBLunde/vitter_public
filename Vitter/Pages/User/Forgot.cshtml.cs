using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Vitter.Database;

namespace Vitter.Pages.User;

public class Forgot : PageModel
{
    public string Error { get; set; }
    public bool Success { get; set; }

    private readonly UserRepository _userRepository;

    public Forgot(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync([FromForm] ForgotPasswordForm form)
    {
        var exists = await _userRepository.UserExists(form.Email);

        if (!exists)
        {
            Error = "No user with this email found";
            return Page();
        }

        //TODO: Implement actual email sending!
        
        Success = true;
        return Page();
    }
}

public class ForgotPasswordForm
{   
    //hortse
    public string Email { get; set; }
}