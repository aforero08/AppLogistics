using AppLogistics.Components.Extensions;
using AppLogistics.Components.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AppLogistics.Components.Mvc;

public class AuthorizationFilter : IResourceFilter
{
    private readonly IAuthorization _authorization;

    public AuthorizationFilter(IAuthorization authorization)
    {
        _authorization = authorization;
    }

    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        if (!context.HttpContext.User.Identity.IsAuthenticated)
        {
            return;
        }

        int? accountId = context.HttpContext.User.Id();
        string area = context.RouteData.Values["area"] as string;
        string action = context.RouteData.Values["action"] as string;
        string controller = context.RouteData.Values["controller"] as string;

        if (_authorization?.IsGrantedFor(accountId, area, controller, action) == false)
        {
            context.Result = new ViewResult
            {
                StatusCode = StatusCodes.Status404NotFound,
                ViewName = "~/Views/Home/NotFound.cshtml"
            };
        }
    }

    public void OnResourceExecuted(ResourceExecutedContext context)
    {
    }
}
