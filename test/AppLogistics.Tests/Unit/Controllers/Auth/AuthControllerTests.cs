using AppLogistics.Components.Mail;
using AppLogistics.Components.Notifications;
using AppLogistics.Objects;
using AppLogistics.Resources;
using AppLogistics.Services;
using AppLogistics.Tests;
using AppLogistics.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AppLogistics.Controllers.Tests;

public class AuthControllerTests : ControllerTests
{
    private AccountRecoveryView accountRecovery;
    private AccountResetView accountReset;
    private AccountLoginView accountLogin;
    private IAccountValidator validator;
    private AuthController controller;
    private IAccountService service;
    private IMailClient mail;

    public AuthControllerTests()
    {
        mail = Substitute.For<IMailClient>();
        service = Substitute.For<IAccountService>();
        validator = Substitute.For<IAccountValidator>();
        controller = Substitute.ForPartsOf<AuthController>(validator, service, mail);
        controller.ControllerContext.HttpContext = Substitute.For<HttpContext>();
        controller.TempData = Substitute.For<ITempDataDictionary>();
        controller.ControllerContext.RouteData = new RouteData();
        controller.Url = Substitute.For<IUrlHelper>();

        accountRecovery = ObjectsFactory.CreateAccountRecoveryView();
        accountReset = ObjectsFactory.CreateAccountResetView();
        accountLogin = ObjectsFactory.CreateAccountLoginView();
    }

    #region Recover()

    [Fact]
    public void Recover_IsLoggedIn_RedirectsToDefault()
    {
        service.IsLoggedIn(controller.User).Returns(true);

        object expected = RedirectToDefault(controller);
        object actual = controller.Recover();

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Recover_ReturnsEmptyView()
    {
        service.IsLoggedIn(controller.User).Returns(false);

        ViewResult actual = controller.Recover() as ViewResult;

        Assert.Null(actual.Model);
    }

    #endregion Recover()

    #region Recover(AccountRecoveryView account)

    [Fact]
    public async Task Recover_Post_IsLoggedIn_RedirectsToDefault()
    {
        service.IsLoggedIn(controller.User).Returns(true);
        validator.CanRecover(accountRecovery).Returns(true);

        object expected = RedirectToDefault(controller);
        object actual = await controller.Recover(null);

        Assert.Same(expected, actual);
    }

    [Fact]
    public async Task Recover_CanNotRecover_ReturnsSameView()
    {
        service.IsLoggedIn(controller.User).Returns(false);
        validator.CanRecover(accountRecovery).Returns(false);

        object actual = (await controller.Recover(accountRecovery) as ViewResult).Model;
        object expected = accountRecovery;

        Assert.Same(expected, actual);
    }

    [Fact]
    public async Task Recover_Account()
    {
        service.IsLoggedIn(controller.User).Returns(false);
        validator.CanRecover(accountRecovery).Returns(true);

        await controller.Recover(accountRecovery);

        service.Received().Recover(accountRecovery);
    }

    [Fact]
    public async Task Recover_SendsRecoveryInformation()
    {
        service.IsLoggedIn(controller.User).Returns(false);
        validator.CanRecover(accountRecovery).Returns(true);
        service.Recover(accountRecovery).Returns("TestToken");

        await controller.Recover(accountRecovery);

        string url = controller.Url.Action("Reset", "Auth", new { token = "TestToken" }, controller.Request.Scheme);
        string subject = Message.For<AccountView>("RecoveryEmailSubject");
        string body = Message.For<AccountView>("RecoveryEmailBody", url);
        string email = accountRecovery.Email;

        await mail.Received().SendFromAdmin(email, "", subject, body);
    }

    [Fact]
    public async Task Recover_NullToken_DoesNotSendRecoveryInformation()
    {
        service.Recover(accountRecovery).ReturnsNull();
        service.IsLoggedIn(controller.User).Returns(false);
        validator.CanRecover(accountRecovery).Returns(true);

        await controller.Recover(accountRecovery);

        await mail.DidNotReceive().SendFromAdmin(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
    }

    [Fact]
    public async Task Recover_AddsRecoveryMessage()
    {
        service.IsLoggedIn(controller.User).Returns(false);
        validator.CanRecover(accountRecovery).Returns(true);
        service.Recover(accountRecovery).Returns("RecoveryToken");

        await controller.Recover(accountRecovery);

        Alert actual = controller.Alerts.Single();

        Assert.Equal(Message.For<AccountView>("RecoveryInformation"), actual.Message);
        Assert.Equal(AlertType.Info, actual.Type);
        Assert.Equal(0, actual.Timeout);
    }

    [Fact]
    public async Task Recover_RedirectsToLogin()
    {
        service.IsLoggedIn(controller.User).Returns(false);
        validator.CanRecover(accountRecovery).Returns(true);
        service.Recover(accountRecovery).Returns("RecoveryToken");

        object expected = RedirectToAction(controller, "Login");
        object actual = await controller.Recover(accountRecovery);

        Assert.Same(expected, actual);
    }

    #endregion Recover(AccountRecoveryView account)

    #region Reset(String token)

    [Fact]
    public void Reset_IsLoggedIn_RedirectsToDefault()
    {
        service.IsLoggedIn(controller.User).Returns(true);

        object expected = RedirectToDefault(controller);
        object actual = controller.Reset("");

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Reset_CanNotReset_RedirectsToRecover()
    {
        service.IsLoggedIn(controller.User).Returns(false);
        validator.CanReset(Arg.Any<AccountResetView>()).Returns(false);

        object expected = RedirectToAction(controller, "Recover");
        object actual = controller.Reset("Token");

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Reset_ReturnsEmptyView()
    {
        service.IsLoggedIn(controller.User).Returns(false);
        validator.CanReset(Arg.Any<AccountResetView>()).Returns(true);

        ViewResult actual = controller.Reset("") as ViewResult;

        Assert.Null(actual.Model);
    }

    #endregion Reset(String token)

    #region Reset(AccountResetView account)

    [Fact]
    public void Reset_Post_IsLoggedIn_RedirectsToDefault()
    {
        service.IsLoggedIn(controller.User).Returns(true);

        object expected = RedirectToDefault(controller);
        object actual = controller.Reset(accountReset);

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Reset_Post_CanNotReset_RedirectsToRecover()
    {
        service.IsLoggedIn(controller.User).Returns(false);
        validator.CanReset(accountReset).Returns(false);

        object expected = RedirectToAction(controller, "Recover");
        object actual = controller.Reset(accountReset);

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Reset_Account()
    {
        service.IsLoggedIn(controller.User).Returns(false);
        validator.CanReset(accountReset).Returns(true);

        controller.Reset(accountReset);

        service.Received().Reset(accountReset);
    }

    [Fact]
    public void Reset_AddsResetMessage()
    {
        service.IsLoggedIn(controller.User).Returns(false);
        validator.CanReset(accountReset).Returns(true);

        controller.Reset(accountReset);

        Alert actual = controller.Alerts.Single();

        Assert.Equal(Message.For<AccountView>("SuccessfulReset"), actual.Message);
        Assert.Equal(AlertType.Success, actual.Type);
        Assert.Equal(4000, actual.Timeout);
    }

    [Fact]
    public void Reset_RedirectsToLogin()
    {
        service.IsLoggedIn(controller.User).Returns(false);
        validator.CanReset(accountReset).Returns(true);

        object expected = RedirectToAction(controller, "Login");
        object actual = controller.Reset(accountReset);

        Assert.Same(expected, actual);
    }

    #endregion Reset(AccountResetView account)

    #region Login(String returnUrl)

    [Fact]
    public void Login_IsLoggedIn_RedirectsToUrl()
    {
        service.IsLoggedIn(controller.User).Returns(true);
        controller.When(sub => sub.RedirectToLocal("/")).DoNotCallBase();
        controller.RedirectToLocal("/").Returns(new RedirectResult("/"));

        object expected = controller.RedirectToLocal("/");
        object actual = controller.Login("/");

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Login_NotLoggedIn_ReturnsEmptyView()
    {
        service.IsLoggedIn(controller.User).Returns(false);

        ViewResult actual = controller.Login("/") as ViewResult;

        Assert.Null(actual.Model);
    }

    #endregion Login(String returnUrl)

    #region Login(AccountLoginView account, String returnUrl)

    [Fact]
    public async Task Login_Post_IsLoggedIn_RedirectsToUrl()
    {
        service.IsLoggedIn(controller.User).Returns(true);
        controller.When(sub => sub.RedirectToLocal("/")).DoNotCallBase();
        controller.RedirectToLocal("/").Returns(new RedirectResult("/"));

        object expected = controller.RedirectToLocal("/");
        object actual = await controller.Login(null, "/");

        Assert.Same(expected, actual);
    }

    [Fact]
    public async Task Login_CanNotLogin_ReturnsSameView()
    {
        validator.CanLogin(accountLogin).Returns(false);

        ActionResult result = await controller.Login(accountLogin, null);

        object actual = (result as ViewResult).Model;
        object expected = accountLogin;

        Assert.Same(expected, actual);
    }

    [Fact]
    public async Task Login_Account()
    {
        validator.CanLogin(accountLogin).Returns(true);
        controller.When(sub => sub.RedirectToLocal(null)).DoNotCallBase();
        controller.RedirectToLocal(null).Returns(new RedirectResult("/"));

        await controller.Login(accountLogin, null);

        await service.Received().Login(controller.HttpContext, accountLogin.Username);
    }

    [Fact]
    public async Task Login_RedirectsToUrl()
    {
        validator.CanLogin(accountLogin).Returns(true);
        controller.When(sub => sub.RedirectToLocal("/")).DoNotCallBase();
        controller.RedirectToLocal("/").Returns(new RedirectResult("/"));

        object actual = await controller.Login(accountLogin, "/");
        object expected = controller.RedirectToLocal("/");

        Assert.Same(expected, actual);
    }

    #endregion Login(AccountLoginView account, String returnUrl)

    #region Logout()

    [Fact]
    public async Task Logout_Account()
    {
        await controller.Logout();

        await service.Received().Logout(controller.HttpContext);
    }

    [Fact]
    public async Task Logout_RedirectsToLogin()
    {
        object expected = RedirectToAction(controller, "Login");
        object actual = await controller.Logout();

        Assert.Same(expected, actual);
    }

    #endregion Logout()
}
