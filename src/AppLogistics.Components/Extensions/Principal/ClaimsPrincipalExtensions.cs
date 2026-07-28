using System.Security.Claims;

namespace AppLogistics.Components.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int? Id(this ClaimsPrincipal principal)
    {
        string id = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(id))
        {
            return null;
        }

        return int.Parse(id);
    }

    public static string Email(this ClaimsPrincipal principal)
    {
        return principal.FindFirst(ClaimTypes.Email)?.Value;
    }

    public static string Username(this ClaimsPrincipal principal)
    {
        return principal.FindFirst(ClaimTypes.Name)?.Value;
    }

    public static void UpdateClaim(this ClaimsPrincipal principal, string type, string value)
    {
        if (principal.Identity is ClaimsIdentity identity)
        {
            identity.TryRemoveClaim(identity.FindFirst(type));
            identity.AddClaim(new Claim(type, value));
        }
    }
}
