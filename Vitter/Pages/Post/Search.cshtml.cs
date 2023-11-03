using Microsoft.AspNetCore.Mvc.RazorPages;
using Vitter.Database;
using Vitter.Models;

namespace Vitter.Pages.Post;

public class Search : PageModel
{
    public string SearchString { get; set; }
    public List<PostInfoDTO> Posts { get; set; }

    private readonly PostRepository _postRepository;

    public Search(PostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task OnGetAsync(string search)
    {
        if (string.IsNullOrEmpty(search))
        {
            Posts = new List<PostInfoDTO>();
            return;
        }

        Posts = await _postRepository.SearchPosts(search);

        SearchString = "<b>Your search for:</b> ";
        SearchString += "<i>" + search + "</i> ";

        if (Posts.Count == 20)
        {
            SearchString += "<b>returned more than 20 results!</b>";
        }
        else
        {
            SearchString += "<b>returned " + Posts.Count + " results!</b>";
        }
    }
}