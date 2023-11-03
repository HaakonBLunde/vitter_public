using Microsoft.AspNetCore.Mvc.Filters;

namespace Vitter.Utils;

public class IsAdminAttribute: Attribute, IAsyncPageFilter
{
    public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
    {
        return Task.CompletedTask;
    }

    public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
    {
        if (!context.HttpContext.IsAuthenticated() || !context.HttpContext.GetSessionData()!.IsAdmin)
        {
            context.HttpContext.Response.StatusCode = 403;
            await context.HttpContext.Response.CompleteAsync();
        }
        else
        {
            await next.Invoke();
        }
    }
}