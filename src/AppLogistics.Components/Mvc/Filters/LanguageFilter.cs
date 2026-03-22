using Microsoft.AspNetCore.Mvc.Filters;

namespace AppLogistics.Components.Mvc;

public class LanguageFilter : IResourceFilter
{
    private readonly ILanguages _languages;

    public LanguageFilter(ILanguages languages)
    {
        _languages = languages;
    }

    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        _languages.Current = _languages[context.RouteData.Values["language"] as string];
    }

    public void OnResourceExecuted(ResourceExecutedContext context)
    {
    }
}
