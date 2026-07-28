using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace AppLogistics.Components.Security.Tests;

[AllowAnonymous]
[ExcludeFromCodeCoverage]
public class AllowAnonymousController : AuthorizedController
{
    [HttpGet]
    [Authorize]
    [AllowAnonymous]
    [AllowUnauthorized]
    public ViewResult AuthorizedAction()
    {
        return null;
    }
}
