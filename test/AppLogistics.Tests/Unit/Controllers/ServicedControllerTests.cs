using AppLogistics.Components.Mvc;
using AppLogistics.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using NSubstitute;
using Xunit;

namespace AppLogistics.Controllers.Tests;

public class ServicedControllerTests : ControllerTests
{
    private ServicedController<IService> controller;
    private IService service;

    public ServicedControllerTests()
    {
        service = Substitute.For<IService>();
        controller = Substitute.ForPartsOf<ServicedController<IService>>(service);

        controller.ControllerContext.RouteData = new RouteData();
        controller.ControllerContext.HttpContext = Substitute.For<HttpContext>();
        controller.HttpContext.RequestServices.GetService(typeof(ILanguages)).Returns(Substitute.For<ILanguages>());
    }

    #region ServicedController(TService service)

    [Fact]
    public void ServicedController_SetsService()
    {
        object actual = controller.Service;
        object expected = service;

        Assert.Same(expected, actual);
    }

    #endregion ServicedController(TService service)

    #region OnActionExecuting(ActionExecutingContext context)

    [Fact]
    public void OnActionExecuting_SetsServiceCurrentAccountId()
    {
        ReturnCurrentAccountId(controller, 1);

        controller.OnActionExecuting(null);

        int expected = controller.CurrentAccountId;
        int actual = service.CurrentAccountId;

        Assert.Equal(expected, actual);
    }

    #endregion OnActionExecuting(ActionExecutingContext context)

    #region Dispose()

    [Fact]
    public void Dispose_Service()
    {
        controller.Dispose();

        service.Received().Dispose();
    }

    [Fact]
    public void Dispose_MultipleTimes()
    {
        controller.Dispose();
        controller.Dispose();
    }

    #endregion Dispose()
}
