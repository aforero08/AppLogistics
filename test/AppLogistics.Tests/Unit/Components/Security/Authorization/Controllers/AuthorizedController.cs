using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace AppLogistics.Components.Security.Tests;

[Authorize]
[ExcludeFromCodeCoverage]
public class AuthorizedController : Controller
{
    [HttpGet]
    public ViewResult Action()
    {
        return null;
    }

    [HttpPost]
    public ViewResult Action(object obj)
    {
        return null;
    }

    [HttpGet]
    [AllowAnonymous]
    public ViewResult AllowAnonymousAction()
    {
        return null;
    }

    [HttpGet]
    [AllowUnauthorized]
    public ViewResult AllowUnauthorizedAction()
    {
        return null;
    }
}
