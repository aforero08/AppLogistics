using AppLogistics.Components.Mvc;
using AppLogistics.Services;
using AppLogistics.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using NSubstitute;
using Xunit;

namespace AppLogistics.Controllers.Tests;

public class ValidatedControllerTests : ControllerTests
{
    private ValidatedController<IValidator, IService> controller;
    private IValidator validator;
    private IService service;

    public ValidatedControllerTests()
    {
        service = Substitute.For<IService>();
        validator = Substitute.For<IValidator>();
        controller = Substitute.ForPartsOf<ValidatedController<IValidator, IService>>(validator, service);

        controller.ControllerContext.RouteData = new RouteData();
        controller.ControllerContext.HttpContext = Substitute.For<HttpContext>();
        controller.HttpContext.RequestServices.GetService(typeof(ILanguages)).Returns(Substitute.For<ILanguages>());
    }

    #region ValidatedController(TService service, TValidator validator)

    [Fact]
    public void ValidatedController_SetsValidator()
    {
        object actual = controller.Validator;
        object expected = validator;

        Assert.Same(expected, actual);
    }

    #endregion ValidatedController(TService service, TValidator validator)

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

    [Fact]
    public void OnActionExecuting_SetsValidatorCurrentAccountId()
    {
        ReturnCurrentAccountId(controller, 1);

        controller.OnActionExecuting(null);

        int expected = controller.CurrentAccountId;
        int actual = validator.CurrentAccountId;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void OnActionExecuting_SetsValidatorAlerts()
    {
        controller.OnActionExecuting(null);

        object expected = controller.Alerts;
        object actual = validator.Alerts;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void OnActionExecuting_SetsModelState()
    {
        controller.OnActionExecuting(null);

        object expected = controller.ModelState;
        object actual = validator.ModelState;

        Assert.Same(expected, actual);
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
    public void Dispose_Validator()
    {
        controller.Dispose();

        validator.Received().Dispose();
    }

    [Fact]
    public void Dispose_MultipleTimes()
    {
        controller.Dispose();
        controller.Dispose();
    }

    #endregion Dispose()
}
