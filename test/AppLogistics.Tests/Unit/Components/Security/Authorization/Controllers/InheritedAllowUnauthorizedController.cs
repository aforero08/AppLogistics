using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace AppLogistics.Components.Security.Tests;

[ExcludeFromCodeCoverage]
public class InheritedAllowUnauthorizedController : AllowUnauthorizedController
{
    [HttpGet]
    public ViewResult InheritanceAction()
    {
        return null;
    }
}
