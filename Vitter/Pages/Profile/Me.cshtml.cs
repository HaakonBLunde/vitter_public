using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Vitter.Database;
using Vitter.Models;
using Vitter.Utils;

namespace Vitter.Pages.Profile;

[Authenticate]
public class Me : PageModel
{
    public List<PostInfoDTO> Posts { get; set; }
    public Models.User UserInfo { get; set; }

    private readonly UserRepository _userRepository;
    private readonly PostRepository _postRepository;

    public Me(UserRepository userRepository, PostRepository postRepository)
    {
        _userRepository = userRepository;
        _postRepository = postRepository;
    }
    
    public async Task<IActionResult> OnGetAsync()
    {
        UserInfo = (await _userRepository.GetUser(HttpContext.GetSessionData()!.UserId))!;
        Posts = await _postRepository.GetLatestPosts(UserInfo.UserId);

        return Page();
    }
}

