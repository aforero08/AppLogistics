using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace AppLogistics.Components.Security.Tests;

[ExcludeFromCodeCoverage]
public class NotAttributedController : Controller
{
    [HttpGet]
    public ViewResult Action()
    {
        return null;
    }
}
