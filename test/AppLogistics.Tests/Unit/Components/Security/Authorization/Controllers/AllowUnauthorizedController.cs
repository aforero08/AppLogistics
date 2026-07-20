using System.Diagnostics.CodeAnalysis;

namespace AppLogistics.Components.Security.Tests;

[AllowUnauthorized]
[ExcludeFromCodeCoverage]
public class AllowUnauthorizedController : AuthorizedController
{
}
