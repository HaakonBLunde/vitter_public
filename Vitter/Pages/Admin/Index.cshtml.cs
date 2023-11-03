using Microsoft.AspNetCore.Mvc.RazorPages;
using Vitter.Utils;

namespace Vitter.Pages.Admin;

[IsAdmin]
public class Index : PageModel
{
    public void OnGet()
    {
    }
}