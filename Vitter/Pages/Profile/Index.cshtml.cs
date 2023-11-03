using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Vitter.Database;
using Vitter.Models;
using Vitter.Utils;

namespace Vitter.Pages.Profile;

public class Index : PageModel
{
    public List<PostInfoDTO> Posts { get; set; }
    public Models.User UserInfo { get; set; }

    private readonly UserRepository _userRepository;
    private readonly PostRepository _postRepository;

    public Index(UserRepository userRepository, PostRepository postRepository)
    {
        _userRepository = userRepository;
        _postRepository = postRepository;
    }

    public async Task<IActionResult> OnGetAsync(string username)
    {
        var user = await _userRepository.GetUserByUsername(username);
        if (user == null)
        {
            return NotFound();
        }

        if (HttpContext.IsAuthenticated() && HttpContext.GetSessionData()!.UserId == user.UserId)
        {
            return RedirectToPage("Me");
        }
        
        UserInfo = user;
        Posts = await _postRepository.GetLatestPosts(user.UserId);

        return Page();
    }
}