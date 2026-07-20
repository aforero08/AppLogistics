using AppLogistics.Components.Notifications;
using AppLogistics.Components.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NSubstitute;
using System.Collections.Generic;
using System.Security.Claims;
using Xunit;

namespace AppLogistics.Controllers.Tests;

public class BaseControllerTests : ControllerTests
{
    private BaseController controller;
    private string controllerName;
    private ActionContext action;
    private string areaName;

    public BaseControllerTests()
    {
        controller = Substitute.ForPartsOf<BaseController>();

        controller.Url = Substitute.For<IUrlHelper>();
        controller.ControllerContext.RouteData = new RouteData();
        controller.TempData = Substitute.For<ITempDataDictionary>();
        controller.ControllerContext.HttpContext = Substitute.For<HttpContext>();
        controller.HttpContext.RequestServices.GetService(typeof(IAuthorization)).Returns(Substitute.For<IAuthorization>());

        action = new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor());

        controllerName = controller.RouteData.Values["controller"] as string;
        areaName = controller.RouteData.Values["area"] as string;
    }

    #region BaseController()

    [Fact]
    public void BaseController_CreatesEmptyAlerts()
    {
        Assert.Empty(controller.Alerts);
    }

    #endregion BaseController()

    #region NotFoundView()

    [Fact]
    public void NotFoundView_ReturnsNotFoundView()
    {
        ViewResult actual = controller.NotFoundView();

        Assert.Equal("~/Views/Home/NotFound.cshtml", actual.ViewName);
        Assert.Equal(StatusCodes.Status404NotFound, controller.Response.StatusCode);
    }

    #endregion NotFoundView()

    #region NotEmptyView(Object model)

    [Fact]
    public void NotEmptyView_NullModel_ReturnsNotFoundView()
    {
        ViewResult expected = NotFoundView(controller);
        ViewResult actual = controller.NotEmptyView(null);

        Assert.Same(expected, actual);
    }

    [Fact]
    public void NotEmptyView_ReturnsModelView()
    {
        object expected = new object();
        object actual = (controller.NotEmptyView(expected) as ViewResult).Model;

        Assert.Same(expected, actual);
    }

    #endregion NotEmptyView(Object model)

    #region RedirectToLocal(String url)

    [Fact]
    public void RedirectToLocal_NotLocalUrl_RedirectsToDefault()
    {
        controller.Url.IsLocalUrl("T").Returns(false);

        object expected = RedirectToDefault(controller);
        object actual = controller.RedirectToLocal("T");

        Assert.Same(expected, actual);
    }

    [Fact]
    public void RedirectToLocal_IsLocalUrl_RedirectsToLocal()
    {
        controller.Url.IsLocalUrl("/").Returns(true);

        string actual = (controller.RedirectToLocal("/") as RedirectResult).Url;
        string expected = "/";

        Assert.Equal(expected, actual);
    }

    #endregion RedirectToLocal(String url)

    #region RedirectToDefault()

    [Fact]
    public void RedirectToDefault_Route()
    {
        RedirectToActionResult actual = controller.RedirectToDefault();

        Assert.Equal("", actual.RouteValues["area"]);
        Assert.Equal("Home", actual.ControllerName);
        Assert.Equal("Index", actual.ActionName);
        Assert.Single(actual.RouteValues);
    }

    #endregion RedirectToDefault()

    #region RedirectToAction(String action, String controller, Object route)

    [Fact]
    public void RedirectToAction_Action_Controller_Route_NotAuthorized_RedirectsToDefault()
    {
        controller.IsAuthorizedFor("Action", "Controller", areaName).Returns(false);

        object expected = RedirectToDefault(controller);
        object actual = controller.RedirectToAction("Action", "Controller", new { id = "Id" });

        Assert.Same(expected, actual);
    }

    [Fact]
    public void RedirectToAction_Action_NullController_NullRoute_RedirectsToAction()
    {
        controller.IsAuthorizedFor("Action", controllerName, areaName).Returns(true);

        RedirectToActionResult actual = controller.RedirectToAction("Action", null, null);

        Assert.Equal(controllerName, actual.ControllerName);
        Assert.Equal("Action", actual.ActionName);
        Assert.Null(actual.RouteValues);
    }

    [Fact]
    public void RedirectToAction_Action_Controller_NullRoute_RedirectsToAction()
    {
        controller.IsAuthorizedFor("Action", "Controller", areaName).Returns(true);

        RedirectToActionResult actual = controller.RedirectToAction("Action", "Controller", null);

        Assert.Equal("Controller", actual.ControllerName);
        Assert.Equal("Action", actual.ActionName);
        Assert.Null(actual.RouteValues);
    }

    [Fact]
    public void RedirectToAction_Action_Controller_Route_RedirectsToAction()
    {
        controller.IsAuthorizedFor("Action", "Controller", "Area").Returns(true);

        RedirectToActionResult actual = controller.RedirectToAction("Action", "Controller", new { area = "Area", id = "Id" });

        Assert.Equal("Controller", actual.ControllerName);
        Assert.Equal("Area", actual.RouteValues["area"]);
        Assert.Equal("Id", actual.RouteValues["id"]);
        Assert.Equal("Action", actual.ActionName);
        Assert.Equal(2, actual.RouteValues.Count);
    }

    #endregion RedirectToAction(String action, String controller, Object route)

    #region IsAuthorizedFor(String action, String controller, String area)

    [Fact]
    public void IsAuthorizedFor_NoAuthorization_ReturnsTrue()
    {
        controller = Substitute.ForPartsOf<BaseController>();

        Assert.Null(controller.Authorization);
        Assert.True(controller.IsAuthorizedFor(null, null, null));
    }

    [Fact]
    public void IsAuthorizedFor_ReturnsAuthorizationResult()
    {
        IAuthorization authorization = controller.HttpContext.RequestServices.GetService<IAuthorization>();
        authorization.IsGrantedFor(controller.CurrentAccountId, "Area", "Controller", "Action").Returns(true);

        controller.OnActionExecuting(null);

        Assert.True(controller.IsAuthorizedFor("Action", "Controller", "Area"));
        Assert.Same(authorization, controller.Authorization);
    }

    #endregion IsAuthorizedFor(String action, String controller, String area)

    #region OnActionExecuting(ActionExecutingContext context)

    [Fact]
    public void OnActionExecuting_SetsAuthorization()
    {
        IAuthorization authorization = controller.HttpContext.RequestServices.GetService<IAuthorization>();

        controller.OnActionExecuting(null);

        object actual = controller.Authorization;
        object expected = authorization;

        Assert.Same(expected, actual);
    }

    [Theory]
    [InlineData("", 0)]
    [InlineData("1", 1)]
    public void OnActionExecuting_SetsCurrentAccountId(string identifier, int accountId)
    {
        controller.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Returns(new Claim(ClaimTypes.NameIdentifier, identifier));

        controller.OnActionExecuting(null);

        int? actual = controller.CurrentAccountId;
        int? expected = accountId;

        Assert.Equal(expected, actual);
    }

    #endregion OnActionExecuting(ActionExecutingContext context)

    #region OnActionExecuted(ActionExecutedContext context)

    [Fact]
    public void OnActionExecuted_JsonResult_NoAlerts()
    {
        controller.Alerts.AddError("Test");
        controller.TempData["Alerts"] = null;
        JsonResult result = new JsonResult("Value");

        controller.OnActionExecuted(new ActionExecutedContext(action, new List<IFilterMetadata>(), null) { Result = result });

        Assert.Null(controller.TempData["Alerts"]);
    }

    [Fact]
    public void OnActionExecuted_NullTempDataAlerts_SetsTempDataAlerts()
    {
        controller.Alerts.AddError("Test");
        controller.TempData["Alerts"] = null;

        controller.OnActionExecuted(new ActionExecutedContext(action, new List<IFilterMetadata>(), null));

        object expected = JsonConvert.SerializeObject(controller.Alerts);
        object actual = controller.TempData["Alerts"];

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void OnActionExecuted_MergesTempDataAlerts()
    {
        Alerts alerts = new Alerts();
        alerts.AddError("Test1");

        controller.TempData["Alerts"] = JsonConvert.SerializeObject(alerts);

        controller.Alerts.AddError("Test2");
        alerts.AddError("Test2");

        controller.OnActionExecuted(new ActionExecutedContext(action, new List<IFilterMetadata>(), null));

        object expected = JsonConvert.SerializeObject(alerts);
        object actual = controller.TempData["Alerts"];

        Assert.Equal(expected, actual);
    }

    #endregion OnActionExecuted(ActionExecutedContext context)
}
