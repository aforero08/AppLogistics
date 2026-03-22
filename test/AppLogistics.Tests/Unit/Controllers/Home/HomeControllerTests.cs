using AppLogistics.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using NSubstitute;
using Xunit;

namespace AppLogistics.Controllers.Tests;

public class HomeControllerTests : ControllerTests
{
    private HomeController controller;
    private IAccountService service;

    public HomeControllerTests()
    {
        service = Substitute.For<IAccountService>();
        controller = Substitute.ForPartsOf<HomeController>(service);

        ActionContext context = new ActionContext(new DefaultHttpContext(), new RouteData(), new ControllerActionDescriptor());
        controller.ControllerContext = new ControllerContext(context);

        ReturnCurrentAccountId(controller, 1);
    }

    #region Index()

    [Fact]
    public void Index_NotActive_RedirectsToLogout()
    {
        service.IsActive(controller.CurrentAccountId).Returns(false);

        object expected = RedirectToAction(controller, "Logout", "Auth");
        object actual = controller.Index();

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Index_ReturnsEmptyView()
    {
        service.IsActive(controller.CurrentAccountId).Returns(true);

        ViewResult actual = controller.Index() as ViewResult;

        Assert.Null(actual.Model);
    }

    #endregion Index()

    #region Error()

    [Fact]
    public void Error_ReturnsEmptyView()
    {
        ViewResult actual = controller.Error() as ViewResult;

        Assert.Equal(StatusCodes.Status500InternalServerError, controller.Response.StatusCode);
        Assert.Null(actual.Model);
    }

    #endregion Error()

    #region NotFound()

    [Fact]
    public void NotFound_NotActive_RedirectsToLogout()
    {
        service.IsLoggedIn(controller.User).Returns(true);
        service.IsActive(controller.CurrentAccountId).Returns(false);

        object expected = RedirectToAction(controller, "Logout", "Auth");
        object actual = controller.NotFound();

        Assert.Same(expected, actual);
    }

    [Theory]
    [InlineData(true, true)]
    [InlineData(false, true)]
    [InlineData(false, false)]
    public void NotFound_ReturnsEmptyView(bool isLoggedIn, bool isActive)
    {
        service.IsActive(controller.CurrentAccountId).Returns(isActive);
        service.IsLoggedIn(controller.User).Returns(isLoggedIn);

        ViewResult actual = controller.NotFound() as ViewResult;

        Assert.Equal(StatusCodes.Status404NotFound, controller.Response.StatusCode);
        Assert.Null(actual.Model);
    }

    #endregion NotFound()
}
