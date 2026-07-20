using System.Security.Claims;
using Xunit;

namespace AppLogistics.Components.Extensions.Tests;

public class ClaimsPrincipalExtensionsTests
{
    #region Id(this ClaimsPrincipal principal)

    [Fact]
    public void Id_NoClaim_ReturnsNull()
    {
        Assert.Null(new ClaimsPrincipal().Id());
    }

    [Theory]
    [InlineData("1", 1)]
    [InlineData("", null)]
    public void Id_ReturnsNameIdentifierClaim(string identifier, int? id)
    {
        ClaimsIdentity identity = new ClaimsIdentity();
        ClaimsPrincipal principal = new ClaimsPrincipal(identity);
        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, identifier));

        int? actual = principal.Id();
        int? expected = id;

        Assert.Equal(expected, actual);
    }

    #endregion Id(this ClaimsPrincipal principal)

    #region Email(this ClaimsPrincipal principal)

    [Fact]
    public void Email_NoClaim_ReturnsNull()
    {
        Assert.Null(new ClaimsPrincipal().Email());
    }

    [Fact]
    public void Email_ReturnsEmailClaim()
    {
        ClaimsIdentity identity = new ClaimsIdentity();
        ClaimsPrincipal principal = new ClaimsPrincipal(identity);
        identity.AddClaim(new Claim(ClaimTypes.Email, "ClaimTypeEmail"));

        string expected = "ClaimTypeEmail";
        string actual = principal.Email();

        Assert.Equal(expected, actual);
    }

    #endregion Email(this ClaimsPrincipal principal)

    #region Username(this ClaimsPrincipal principal)

    [Fact]
    public void Username_NoClaim_ReturnsNull()
    {
        Assert.Null(new ClaimsPrincipal().Username());
    }

    [Fact]
    public void Username_ReturnsNameClaim()
    {
        ClaimsIdentity identity = new ClaimsIdentity();
        ClaimsPrincipal principal = new ClaimsPrincipal(identity);
        identity.AddClaim(new Claim(ClaimTypes.Name, "ClaimTypeName"));

        string actual = principal.Username();
        string expected = "ClaimTypeName";

        Assert.Equal(expected, actual);
    }

    #endregion Username(this ClaimsPrincipal principal)

    #region UpdateClaim(this ClaimsPrincipal principal)

    [Fact]
    public void UpdateClaim_New()
    {
        ClaimsIdentity identity = new ClaimsIdentity();
        ClaimsPrincipal principal = new ClaimsPrincipal(identity);

        principal.UpdateClaim(ClaimTypes.Name, "Test");

        string actual = principal.FindFirst(ClaimTypes.Name).Value;
        string expected = "Test";

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void UpdateClaim_Existing()
    {
        ClaimsIdentity identity = new ClaimsIdentity();
        ClaimsPrincipal principal = new ClaimsPrincipal(identity);
        identity.AddClaim(new Claim(ClaimTypes.Name, "ClaimTypeName"));

        principal.UpdateClaim(ClaimTypes.Name, "Test");

        string actual = principal.FindFirst(ClaimTypes.Name).Value;
        string expected = "Test";

        Assert.Equal(expected, actual);
    }

    #endregion UpdateClaim(this ClaimsPrincipal principal)
}
