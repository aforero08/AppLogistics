using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Tests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System;
using System.Reflection;
using Xunit;

namespace AppLogistics.Components.Security.Tests;

public class AuthorizationTests
{
    private DbContext context;
    private Authorization authorization;

    public AuthorizationTests()
    {
        context = TestFixture.Create();
        IServiceProvider services = Substitute.For<IServiceProvider>();
        services.GetService(typeof(IUnitOfWork)).Returns(info => new UnitOfWork(TestFixture.Create(), TestFixture.Mapper));
        IServiceScope scope = Substitute.For<IServiceScope>();
        scope.ServiceProvider.Returns(services);
        IServiceScopeFactory scopeFactory = Substitute.For<IServiceScopeFactory>();
        scopeFactory.CreateScope().Returns(scope);

        authorization = new Authorization(Assembly.GetExecutingAssembly(), scopeFactory);
    }

    #region IsGrantedFor(Int32? accountId, String area, String controller, String action)

    [Fact]
    public void IsGrantedFor_AuthorizesControllerByIgnoringCase()
    {
        int accountId = CreateAccountWithPermissionFor("Area", "Authorized", "Action");

        Assert.True(authorization.IsGrantedFor(accountId, "Area", "AUTHORIZED", "Action"));
    }

    [Fact]
    public void IsGrantedFor_DoesNotAuthorizeControllerByIgnoringCase()
    {
        int accountId = CreateAccountWithPermissionFor("Test", "Test", "Test");

        Assert.False(authorization.IsGrantedFor(accountId, "Area", "AUTHORIZED", "Action"));
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void IsGrantedFor_AuthorizesControllerWithoutArea(string area)
    {
        int accountId = CreateAccountWithPermissionFor(null, "Authorized", "Action");

        Assert.True(authorization.IsGrantedFor(accountId, area, "Authorized", "Action"));
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void IsGrantedFor_DoesNotAuthorizeControllerWithoutArea(string area)
    {
        int accountId = CreateAccountWithPermissionFor(null, "Test", "Test");

        Assert.False(authorization.IsGrantedFor(accountId, area, "Authorized", "Action"));
    }

    [Fact]
    public void IsGrantedFor_AuthorizesControllerWithArea()
    {
        int accountId = CreateAccountWithPermissionFor("Area", "Authorized", "Action");

        Assert.True(authorization.IsGrantedFor(accountId, "Area", "Authorized", "Action"));
    }

    [Fact]
    public void IsGrantedFor_DoesNotAuthorizeControllerWithArea()
    {
        int accountId = CreateAccountWithPermissionFor("Test", "Test", "Test");

        Assert.False(authorization.IsGrantedFor(accountId, "Area", "Authorized", "Action"));
    }

    [Fact]
    public void IsGrantedFor_AuthorizesGetAction()
    {
        int accountId = CreateAccountWithPermissionFor("Area", "Authorized", "AuthorizedGetAction");

        Assert.True(authorization.IsGrantedFor(accountId, "Area", "Authorized", "AuthorizedGetAction"));
    }

    [Fact]
    public void IsGrantedFor_DoesNotAuthorizeGetAction()
    {
        int accountId = CreateAccountWithPermissionFor("Test", "Test", "Test");

        Assert.False(authorization.IsGrantedFor(accountId, "Area", "Authorized", "AuthorizedGetAction"));
    }

    [Fact]
    public void IsGrantedFor_AuthorizesNamedGetAction()
    {
        int accountId = CreateAccountWithPermissionFor("Area", "Authorized", "AuthorizedNamedGetAction");

        Assert.True(authorization.IsGrantedFor(accountId, "Area", "Authorized", "AuthorizedNamedGetAction"));
    }

    [Fact]
    public void IsGrantedFor_DoesNotAuthorizeNamedGetAction()
    {
        int accountId = CreateAccountWithPermissionFor("Test", "Test", "Test");

        Assert.False(authorization.IsGrantedFor(accountId, "Area", "Authorized", "AuthorizedNamedGetAction"));
    }

    [Fact]
    public void IsGrantedFor_AuthorizesNotExistingAction()
    {
        Assert.True(authorization.IsGrantedFor(null, null, "Authorized", "Test"));
    }

    [Fact]
    public void IsGrantedFor_AuthorizesNonGetAction()
    {
        int accountId = CreateAccountWithPermissionFor("Area", "Authorized", "AuthorizedPostAction");

        Assert.True(authorization.IsGrantedFor(accountId, "Area", "Authorized", "AuthorizedPostAction"));
    }

    [Fact]
    public void IsGrantedFor_DoesNotAuthorizeNonGetAction()
    {
        int accountId = CreateAccountWithPermissionFor("Test", "Test", "Test");

        Assert.False(authorization.IsGrantedFor(accountId, "Area", "Authorized", "AuthorizedPostAction"));
    }

    [Fact]
    public void IsGrantedFor_AuthorizesNamedNonGetAction()
    {
        int accountId = CreateAccountWithPermissionFor("Area", "Authorized", "AuthorizedNamedPostAction");

        Assert.True(authorization.IsGrantedFor(accountId, "Area", "Authorized", "AuthorizedNamedPostAction"));
    }

    [Fact]
    public void IsGrantedFor_DoesNotAuthorizeNamedNonGetAction()
    {
        int accountId = CreateAccountWithPermissionFor("Area", "Test", "Test");

        Assert.False(authorization.IsGrantedFor(accountId, "Area", "Authorized", "AuthorizedNamedPostAction"));
    }

    [Fact]
    public void IsGrantedFor_AuthorizesActionAsAction()
    {
        int accountId = CreateAccountWithPermissionFor("Area", "Authorized", "Action");

        Assert.True(authorization.IsGrantedFor(accountId, "Area", "Authorized", "AuthorizedAsAction"));
    }

    [Fact]
    public void IsGrantedFor_DoesNotAuthorizeActionAsAction()
    {
        int accountId = CreateAccountWithPermissionFor(null, "Authorized", "Action");

        Assert.False(authorization.IsGrantedFor(accountId, "Area", "Authorized", "AuthorizedAsAction"));
    }

    [Fact]
    public void IsGrantedFor_AuthorizesActionAsSelf()
    {
        int accountId = CreateAccountWithPermissionFor("Area", "Authorized", "AuthorizedAsSelf");

        Assert.True(authorization.IsGrantedFor(accountId, "Area", "Authorized", "AuthorizedAsSelf"));
    }

    [Fact]
    public void IsGrantedFor_DoesNotAuthorizeActionAsSelf()
    {
        int accountId = CreateAccountWithPermissionFor("Area", "Test", "Test");

        Assert.False(authorization.IsGrantedFor(accountId, "Area", "Authorized", "AuthorizedAsSelf"));
    }

    [Fact]
    public void IsGrantedFor_AuthorizesActionAsOtherAction()
    {
        int accountId = CreateAccountWithPermissionFor(null, "InheritedAuthorized", "InheritanceAction");

        Assert.True(authorization.IsGrantedFor(accountId, "Area", "Authorized", "AuthorizedAsOtherAction"));
    }

    [Fact]
    public void IsGrantedFor_DoesNotAuthorizeActionAsOtherAction()
    {
        int accountId = CreateAccountWithPermissionFor("Area", "Authorized", "AuthorizedAsOtherAction");

        Assert.False(authorization.IsGrantedFor(accountId, "Area", "Authorized", "AuthorizedAsOtherAction"));
    }

    [Fact]
    public void IsGrantedFor_AuthorizesEmptyAreaAsNull()
    {
        int accountId = CreateAccountWithPermissionFor(null, "Authorized", "Action");

        Assert.True(authorization.IsGrantedFor(accountId, "", "Authorized", "Action"));
    }

    [Fact]
    public void IsGrantedFor_DoesNotAuthorizeEmptyAreaAsNull()
    {
        int accountId = CreateAccountWithPermissionFor(null, "Test", "Test");

        Assert.False(authorization.IsGrantedFor(accountId, "", "Authorized", "Action"));
    }

    [Fact]
    public void IsGrantedFor_AuthorizesAuthorizedAction()
    {
        int accountId = CreateAccountWithPermissionFor(null, "AllowAnonymous", "AuthorizedAction");

        Assert.True(authorization.IsGrantedFor(accountId, null, "AllowAnonymous", "AuthorizedAction"));
    }

    [Fact]
    public void IsGrantedFor_AuthorizesAllowAnonymousAction()
    {
        int accountId = CreateAccountWithPermissionFor(null, "Test", "Test");

        Assert.True(authorization.IsGrantedFor(accountId, null, "Authorized", "AllowAnonymousAction"));
    }

    [Fact]
    public void IsGrantedFor_AuthorizesAllowUnauthorizedAction()
    {
        int accountId = CreateAccountWithPermissionFor(null, "Test", "Test");

        Assert.True(authorization.IsGrantedFor(accountId, null, "Authorized", "AllowUnauthorizedAction"));
    }

    [Fact]
    public void IsGrantedFor_AuthorizesAuthorizedController()
    {
        int accountId = CreateAccountWithPermissionFor(null, "Authorized", "Action");

        Assert.True(authorization.IsGrantedFor(accountId, null, "Authorized", "Action"));
    }

    [Fact]
    public void IsGrantedFor_DoesNotAuthorizeAuthorizedController()
    {
        int accountId = CreateAccountWithPermissionFor(null, "Test", "Test");

        Assert.False(authorization.IsGrantedFor(accountId, null, "Authorized", "Action"));
    }

    [Fact]
    public void IsGrantedFor_AuthorizesAllowAnonymousController()
    {
        int accountId = CreateAccountWithPermissionFor(null, "Test", "Test");

        Assert.True(authorization.IsGrantedFor(accountId, null, "AllowAnonymous", "Action"));
    }

    [Fact]
    public void IsGrantedFor_AuthorizesAllowUnauthorizedController()
    {
        int accountId = CreateAccountWithPermissionFor(null, "Test", "Test");

        Assert.True(authorization.IsGrantedFor(accountId, null, "AllowUnauthorized", "Action"));
    }

    [Fact]
    public void IsGrantedFor_AuthorizesInheritedAuthorizedController()
    {
        int accountId = CreateAccountWithPermissionFor(null, "InheritedAuthorized", "InheritanceAction");

        Assert.True(authorization.IsGrantedFor(accountId, null, "InheritedAuthorized", "InheritanceAction"));
    }

    [Fact]
    public void IsGrantedFor_DoesNotAuthorizeInheritedAuthorizedController()
    {
        int accountId = CreateAccountWithPermissionFor(null, "Test", "Test");

        Assert.False(authorization.IsGrantedFor(accountId, null, "InheritedAuthorized", "InheritanceAction"));
    }

    [Fact]
    public void IsGrantedFor_AuthorizesInheritedAllowAnonymousController()
    {
        int accountId = CreateAccountWithPermissionFor(null, "Test", "Test");

        Assert.True(authorization.IsGrantedFor(accountId, null, "InheritedAllowAnonymous", "InheritanceAction"));
    }

    [Fact]
    public void IsGrantedFor_AuthorizesInheritedAllowUnauthorizedController()
    {
        int accountId = CreateAccountWithPermissionFor(null, "Test", "Test");

        Assert.True(authorization.IsGrantedFor(accountId, null, "InheritedAllowUnauthorized", "InheritanceAction"));
    }

    [Fact]
    public void IsGrantedFor_AuthorizesNotAttributedController()
    {
        int accountId = CreateAccountWithPermissionFor(null, "Test", "Test");

        Assert.True(authorization.IsGrantedFor(accountId, null, "NotAttributed", "Action"));
    }

    [Fact]
    public void IsGrantedFor_DoesNotAuthorizeNotExistingAccount()
    {
        CreateAccountWithPermissionFor("Area", "Authorized", "Action");

        Assert.False(authorization.IsGrantedFor(0, "Area", "Authorized", "Action"));
    }

    [Fact]
    public void IsGrantedFor_DoesNotAuthorizeLockedAccount()
    {
        int accountId = CreateAccountWithPermissionFor("Area", "Authorized", "Action", isLocked: true);

        Assert.False(authorization.IsGrantedFor(accountId, "Area", "Authorized", "Action"));
    }

    [Fact]
    public void IsGrantedFor_DoesNotAuthorizeNullAccount()
    {
        CreateAccountWithPermissionFor(null, "Authorized", "Action");

        Assert.False(authorization.IsGrantedFor(null, null, "Authorized", "Action"));
    }

    [Fact]
    public void IsGrantedFor_AuthorizesByIgnoringCase()
    {
        int accountId = CreateAccountWithPermissionFor("Area", "Authorized", "Action");

        Assert.True(authorization.IsGrantedFor(accountId, "area", "authorized", "action"));
    }

    [Fact]
    public void IsGrantedFor_DoesNotAuthorizeByIgnoringCase()
    {
        int accountId = CreateAccountWithPermissionFor("Test", "Test", "Test");

        Assert.False(authorization.IsGrantedFor(accountId, "area", "authorized", "action"));
    }

    [Fact]
    public void IsGrantedFor_CachesAccountPermissions()
    {
        int accountId = CreateAccountWithPermissionFor(null, "Authorized", "Action");

        context.Database.EnsureDeleted();

        Assert.True(authorization.IsGrantedFor(accountId, null, "Authorized", "Action"));
    }

    #endregion IsGrantedFor(Int32? accountId, String area, String controller, String action)

    #region Refresh()

    [Fact]
    public void Refresh_Permissions()
    {
        int accountId = CreateAccountWithPermissionFor("Area", "Authorized", "Action");
        Assert.True(authorization.IsGrantedFor(accountId, "Area", "Authorized", "Action"));

        context.Database.EnsureDeleted();

        authorization.Refresh();

        Assert.False(authorization.IsGrantedFor(accountId, "Area", "Authorized", "Action"));
    }

    #endregion Refresh()

    #region Test helpers

    private int CreateAccountWithPermissionFor(string area, string controller, string action, bool isLocked = false)
    {
        int id = Math.Abs(Guid.NewGuid().GetHashCode());
        if (id == 0)
        {
            id = 1;
        }

        RolePermission rolePermission = ObjectsFactory.CreateRolePermission(id);
        Account account = ObjectsFactory.CreateAccount(id);
        account.Role.Permissions.Add(rolePermission);
        rolePermission.Role = account.Role;
        account.IsLocked = isLocked;

        rolePermission.Permission.Controller = controller;
        rolePermission.Permission.Action = action;
        rolePermission.Permission.Area = area;

        context.Add(account);
        context.SaveChanges();

        authorization.Refresh();

        return account.Id;
    }

    #endregion Test helpers
}
