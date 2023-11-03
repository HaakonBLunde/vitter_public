using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Vitter.Database;
using Vitter.Utils;

namespace Vitter.Pages.Profile;

[Authenticate]
public class Edit : PageModel
{
    [BindProperty] public ProfileUpdateForm Form { get; set; }

    public string Error { get; set; }

    private readonly UserRepository _userRepository;

    public Edit(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IActionResult> OnGetAsync(long id)
    {
        var user = await _userRepository.GetUser(id);
        if (user == null)
        {
            return NotFound();
        }

        Form = new ProfileUpdateForm()
        {
            About = user.About,
            PhotoUri = user.PhotoUri
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(long id)
    {
        if (!string.IsNullOrEmpty(Form.PhotoUri))
        {
            if (!await ValidatePhotoUri(Form.PhotoUri))
            {
                Error = $"{Form.PhotoUri} is not a reachable uri";
                return Page();
            }
        }

        await _userRepository.UpdateUser(id, Form.About, Form.PhotoUri);

        return RedirectToPage("Me");
    }

    private async Task<bool> ValidatePhotoUri(string uri)
    {
        var client = new HttpClient();
        try
        {
            var res = await client.GetAsync(uri);
            return res.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            return false;
        }
    }
}

public class ProfileUpdateForm
{
    public string About { get; set; }
    public string PhotoUri { get; set; }
}