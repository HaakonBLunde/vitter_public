using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Vitter.Database;
using Vitter.Utils;

namespace Vitter.Pages.Post;

[Authenticate]
public class New : PageModel
{
    private readonly PostRepository _postRepository;

    public New(PostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync([FromForm] NewPostForm form)
    {
        if (!string.IsNullOrEmpty(form.Content))
        {
            //Filter away those nasty script tags!
            form.Content = form.Content.Replace("script", "");

            await _postRepository.CreatePost(HttpContext.GetSessionData()!.UserId, form.Content);
        }

        return RedirectToPage("/Index");
    }
}

public class NewPostForm
{
    public string Content { get; set; }
}