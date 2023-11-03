using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Vitter.Database;
using Vitter.Models;
using Vitter.Utils;

namespace Vitter.Pages;

public class IndexModel : PageModel
{
    public List<PostInfoDTO>? Posts { get; set; }
    public Models.User? UserInfo { get; set; }

    private readonly ILogger<IndexModel> _logger;
    private readonly PostRepository _postRepository;
    private readonly UserRepository _userRepository;

    public IndexModel(ILogger<IndexModel> logger, PostRepository postRepository, UserRepository userRepository)
    {
        _logger = logger;
        _postRepository = postRepository;
        _userRepository = userRepository;
    }

    public async Task OnGetAsync()
    {
        if (HttpContext.IsAuthenticated())
        {
            Posts = await _postRepository.GetLatestPosts();
            UserInfo = await _userRepository.GetUser(HttpContext.GetSessionData()!.UserId);
        }
    }
}