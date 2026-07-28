using AppLogistics.Components.Extensions;
using AppLogistics.Components.Security;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AppLogistics.Components.Mvc;

[HtmlTargetElement("authorize", Attributes = "action")]
public class AuthorizeTagHelper : TagHelper
{
    public string Area { get; set; }
    public string Action { get; set; }
    public string Controller { get; set; }

    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; }

    private readonly IAuthorization _authorization;

    public AuthorizeTagHelper(IAuthorization authorization)
    {
        _authorization = authorization;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = null;

        int? accountId = ViewContext.HttpContext.User.Id();
        string area = Area ?? ViewContext.RouteData.Values["area"] as string;
        string action = Action ?? ViewContext.RouteData.Values["action"] as string;
        string controller = Controller ?? ViewContext.RouteData.Values["controller"] as string;

        if (_authorization?.IsGrantedFor(accountId, area, controller, action) == false)
        {
            output.SuppressOutput();
        }
    }
}
