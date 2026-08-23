using AppLogistics.Components.Security;
using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Tests;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Xunit;

namespace AppLogistics.Services.Tests;

public class AccountServiceTests
{
    private HttpContext httpContext;
    private AccountService service;
    private DbContext context;
    private Account account;
    private IHasher hasher;

    public AccountServiceTests()
    {
        context = TestFixture.Create().Drop();
        hasher = Substitute.For<IHasher>();
        httpContext = new DefaultHttpContext();
        service = new AccountService(new UnitOfWork(TestFixture.Create(), TestFixture.Mapper), hasher);
        hasher.HashPassword(Arg.Any<string>()).Returns(info => info.Arg<string>() + "Hashed");

        context.Add(account = ObjectsFactory.CreateAccount());
        context.SaveChanges();

        service.CurrentAccountId = account.Id;
    }

    #region Get<TView>(Int32 id)

    [Fact]
    public void Get_ReturnsViewById()
    {
        AccountView actual = service.Get<AccountView>(account.Id);
        AccountView expected = TestFixture.Mapper.Map<AccountView>(account);

        Assert.Equal(expected.CreationDate, actual.CreationDate);
        Assert.Equal(expected.RoleTitle, actual.RoleTitle);
        Assert.Equal(expected.IsLocked, actual.IsLocked);
        Assert.Equal(expected.Username, actual.Username);
        Assert.Equal(expected.Email, actual.Email);
        Assert.Equal(expected.Id, actual.Id);
    }

    #endregion Get<TView>(Int32 id)

    #region GetViews()

    [Fact]
    public void GetViews_ReturnsAccountViews()
    {
        AccountView[] actual = service.GetViews().ToArray();
        AccountView[] expected = context
            .Set<Account>()
            .AsNoTracking()
            .ProjectTo<AccountView>(TestFixture.Mapper.ConfigurationProvider)
            .OrderByDescending(view => view.Id)
            .ToArray();

        for (int i = 0; i < expected.Length || i < actual.Length; i++)
        {
            Assert.Equal(expected[i].CreationDate, actual[i].CreationDate);
            Assert.Equal(expected[i].RoleTitle, actual[i].RoleTitle);
            Assert.Equal(expected[i].IsLocked, actual[i].IsLocked);
            Assert.Equal(expected[i].Username, actual[i].Username);
            Assert.Equal(expected[i].Email, actual[i].Email);
            Assert.Equal(expected[i].Id, actual[i].Id);
        }
    }

    #endregion GetViews()

    #region IsLoggedIn(IPrincipal user)

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void IsLoggedIn_ReturnsIsAuthenticated(bool isAuthenticated)
    {
        IPrincipal user = Substitute.For<IPrincipal>();
        user.Identity.IsAuthenticated.Returns(isAuthenticated);

        bool actual = service.IsLoggedIn(user);
        bool expected = isAuthenticated;

        Assert.Equal(expected, actual);
    }

    #endregion IsLoggedIn(IPrincipal user)

    #region IsActive(Int32 id)

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void IsActive_ReturnsAccountState(bool isLocked)
    {
        account.IsLocked = isLocked;
        context.Update(account);
        context.SaveChanges();

        bool actual = service.IsActive(account.Id);
        bool expected = !isLocked;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void IsActive_NoAccount_ReturnsFalse()
    {
        Assert.False(service.IsActive(0));
    }

    #endregion IsActive(Int32 id)

    #region Recover(AccountRecoveryView view)

    [Fact]
    public void Recover_NoEmail_ReturnsNull()
    {
        AccountRecoveryView view = ObjectsFactory.CreateAccountRecoveryView();
        view.Email = "not@existing.email";

        Assert.Null(service.Recover(view));
    }

    [Fact]
    public void Recover_Information()
    {
        AccountRecoveryView view = ObjectsFactory.CreateAccountRecoveryView();
        account.RecoveryTokenExpirationDate = DateTime.Now.AddMinutes(30);
        string oldToken = account.RecoveryToken;
        view.Email = view.Email.ToUpper();

        account.RecoveryToken = service.Recover(view);

        Account actual = context.Set<Account>().AsNoTracking().Single();
        Account expected = account;

        Assert.InRange(actual.RecoveryTokenExpirationDate.Value.Ticks,
            expected.RecoveryTokenExpirationDate.Value.Ticks - TimeSpan.TicksPerSecond,
            expected.RecoveryTokenExpirationDate.Value.Ticks + TimeSpan.TicksPerSecond);
        Assert.Equal(expected.RecoveryToken, actual.RecoveryToken);
        Assert.Equal(expected.CreationDate, actual.CreationDate);
        Assert.Equal(expected.IsLocked, actual.IsLocked);
        Assert.Equal(expected.Passhash, actual.Passhash);
        Assert.Equal(expected.Username, actual.Username);
        Assert.NotEqual(oldToken, actual.RecoveryToken);
        Assert.Equal(expected.RoleId, actual.RoleId);
        Assert.Equal(expected.Email, actual.Email);
        Assert.Equal(expected.Id, actual.Id);
        Assert.NotNull(actual.RecoveryToken);
    }

    #endregion Recover(AccountRecoveryView view)

    #region Reset(AccountResetView view)

    [Fact]
    public void Reset_Account()
    {
        AccountResetView view = ObjectsFactory.CreateAccountResetView();

        service.Reset(view);

        Account actual = context.Set<Account>().AsNoTracking().Single();
        Account expected = account;

        Assert.Equal(hasher.HashPassword(view.NewPassword), actual.Passhash);
        Assert.Equal(expected.CreationDate, actual.CreationDate);
        Assert.Equal(expected.IsLocked, actual.IsLocked);
        Assert.Equal(expected.Username, actual.Username);
        Assert.Null(actual.RecoveryTokenExpirationDate);
        Assert.Equal(expected.RoleId, actual.RoleId);
        Assert.Equal(expected.Email, actual.Email);
        Assert.Equal(expected.Id, actual.Id);
        Assert.Null(actual.RecoveryToken);
    }

    #endregion Reset(AccountResetView view)

    #region Create(AccountCreateView view)

    [Fact]
    public void Create_Account()
    {
        AccountCreateView view = ObjectsFactory.CreateAccountCreateView(1);
        view.Email = view.Email.ToUpper();
        view.RoleId = account.RoleId;

        service.Create(view);

        Account actual = context.Set<Account>().AsNoTracking().Single(model => model.Id != account.Id);
        AccountCreateView expected = view;

        Assert.Equal(hasher.HashPassword(expected.Password), actual.Passhash);
        Assert.Equal(expected.CreationDate, actual.CreationDate);
        Assert.Equal(expected.Email.ToLower(), actual.Email);
        Assert.Equal(expected.Username, actual.Username);
        Assert.Null(actual.RecoveryTokenExpirationDate);
        Assert.Equal(expected.RoleId, actual.RoleId);
        Assert.Null(actual.RecoveryToken);
        Assert.False(actual.IsLocked);
    }

    #endregion Create(AccountCreateView view)

    #region Edit(AccountEditView view)

    [Fact]
    public void Edit_Account()
    {
        AccountEditView view = ObjectsFactory.CreateAccountEditView(account.Id);
        view.IsLocked = account.IsLocked = !account.IsLocked;
        view.Username = account.Username + "Test";
        view.RoleId = account.RoleId = null;
        view.Email = account.Email + "s";

        service.Edit(view);

        Account actual = context.Set<Account>().AsNoTracking().Single();
        Account expected = account;

        Assert.Equal(expected.RecoveryTokenExpirationDate, actual.RecoveryTokenExpirationDate);
        Assert.Equal(expected.RecoveryToken, actual.RecoveryToken);
        Assert.Equal(expected.CreationDate, actual.CreationDate);
        Assert.Equal(expected.IsLocked, actual.IsLocked);
        Assert.Equal(expected.Username, actual.Username);
        Assert.Equal(expected.Passhash, actual.Passhash);
        Assert.Equal(expected.RoleId, actual.RoleId);
        Assert.Equal(expected.Email, actual.Email);
        Assert.Equal(expected.Id, actual.Id);
    }

    #endregion Edit(AccountEditView view)

    #region Edit(ClaimsPrincipal user, ProfileEditView view)

    [Fact]
    public void Edit_Profile()
    {
        ProfileEditView view = ObjectsFactory.CreateProfileEditView();
        account.Passhash = hasher.HashPassword(view.NewPassword);
        view.Username = account.Username += "Test";
        view.Email = account.Email += "Test";

        service.Edit(httpContext.User, view);

        Account actual = context.Set<Account>().AsNoTracking().Single();
        Account expected = account;

        Assert.Equal(expected.RecoveryTokenExpirationDate, actual.RecoveryTokenExpirationDate);
        Assert.Equal(expected.RecoveryToken, actual.RecoveryToken);
        Assert.Equal(expected.CreationDate, actual.CreationDate);
        Assert.Equal(expected.Email.ToLower(), actual.Email);
        Assert.Equal(expected.IsLocked, actual.IsLocked);
        Assert.Equal(expected.Username, actual.Username);
        Assert.Equal(expected.Passhash, actual.Passhash);
        Assert.Equal(expected.RoleId, actual.RoleId);
        Assert.Equal(expected.Id, actual.Id);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void Edit_NullOrEmptyNewPassword_DoesNotEditPassword(string newPassword)
    {
        ProfileEditView view = ObjectsFactory.CreateProfileEditView();
        view.NewPassword = newPassword;

        service.Edit(httpContext.User, view);

        string actual = context.Set<Account>().AsNoTracking().Single().Passhash;
        string expected = account.Passhash;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Edit_UpdatesClaims()
    {
        httpContext.User.AddIdentity(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Email, "test@email.com"),
            new Claim(ClaimTypes.Name, "TestName")
        }));

        ProfileEditView view = ObjectsFactory.CreateProfileEditView();
        view.Username = account.Username += "Test";
        view.Email = account.Email += "Test";

        service.Edit(httpContext.User, view);

        Account expected = account;
        ClaimsPrincipal actual = httpContext.User;

        Assert.Equal(expected.Username, actual.FindFirst(ClaimTypes.Name).Value);
        Assert.Equal(expected.Email.ToLower(), actual.FindFirst(ClaimTypes.Email).Value);
    }

    #endregion Edit(ClaimsPrincipal user, ProfileEditView view)

    #region Delete(Int32 id)

    [Fact]
    public void Delete_Account()
    {
        service.Delete(account.Id);

        Assert.Empty(context.Set<Account>().AsNoTracking());
    }

    #endregion Delete(Int32 id)

    #region Login(HttpContext context, String username)

    [Fact]
    public async Task Login_Account()
    {
        httpContext = Substitute.For<HttpContext>();
        IAuthenticationService authentication = Substitute.For<IAuthenticationService>();
        httpContext.RequestServices.GetService(typeof(IAuthenticationService)).Returns(authentication);

        await service.Login(httpContext, account.Username.ToUpper());

        await authentication.Received().SignInAsync(httpContext, "Cookies", Arg.Is<ClaimsPrincipal>(principal =>
            principal.FindFirst(ClaimTypes.NameIdentifier).Value == account.Id.ToString()
            && principal.FindFirst(ClaimTypes.Name).Value == account.Username
            && principal.FindFirst(ClaimTypes.Email).Value == account.Email
            && principal.Identity.AuthenticationType == "Password"), null);
    }

    #endregion Login(HttpContext context, String username)

    #region Logout(HttpContext context)

    [Fact]
    public async Task Logout_Account()
    {
        httpContext = Substitute.For<HttpContext>();
        IAuthenticationService authentication = Substitute.For<IAuthenticationService>();
        httpContext.RequestServices.GetService(typeof(IAuthenticationService)).Returns(authentication);

        await service.Logout(httpContext);

        await authentication.Received().SignOutAsync(httpContext, "Cookies", null);
    }

    #endregion Logout(HttpContext context)
}
