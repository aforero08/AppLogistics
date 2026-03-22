using AppLogistics.Controllers.Tests;
using AppLogistics.Objects;
using AppLogistics.Services;
using AppLogistics.Tests;
using AppLogistics.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using NSubstitute;
using System.Linq;
using Xunit;

namespace AppLogistics.Controllers.Operation.Tests;

public class ServicesControllerTests : ControllerTests
{
    private ServicesController controller;
    private IServiceValidator validator;
    private IServiceService serviceService;
    private ServiceView serviceView;
    private ServiceCreateEditView serviceCreateEditView;

    public ServicesControllerTests()
    {
        validator = Substitute.For<IServiceValidator>();
        serviceService = Substitute.For<IServiceService>();

        serviceView = ObjectsFactory.CreateServiceView();
        serviceCreateEditView = ObjectsFactory.CreateServiceCreateEditView();

        controller = Substitute.ForPartsOf<ServicesController>(validator, serviceService);
        controller.ControllerContext.RouteData = new RouteData();
    }

    #region Index()

    [Fact]
    public void Index_ReturnsServiceViews()
    {
        serviceService.GetViews().Returns(new ServiceView[0].AsQueryable());

        object actual = controller.Index().ViewData.Model;
        object expected = serviceService.GetViews();

        Assert.Same(expected, actual);
    }

    #endregion

    #region Create()

    [Fact]
    public void Create_ReturnsEmptyView()
    {
        ViewDataDictionary actual = controller.Create().ViewData;

        Assert.Null(actual.Model);
    }

    #endregion

    #region Create(ServiceView service)

    [Fact]
    public void Create_ProtectsFromOverpostingId()
    {
        ProtectsFromOverpostingId(controller, "Create");
    }

    [Fact]
    public void Create_CanNotCreate_ReturnsSameView()
    {
        validator.CanCreate(serviceCreateEditView).Returns(false);

        object actual = (controller.Create(serviceCreateEditView) as ViewResult).ViewData.Model;
        object expected = serviceCreateEditView;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Create_Service()
    {
        validator.CanCreate(serviceCreateEditView).Returns(true);

        controller.Create(serviceCreateEditView);

        serviceService.Received().Create(serviceCreateEditView);
    }

    [Fact]
    public void Create_RedirectsToIndex()
    {
        validator.CanCreate(serviceCreateEditView).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.Create(serviceCreateEditView);

        Assert.Same(expected, actual);
    }

    #endregion

    #region Details(String id)

    [Fact]
    public void Details_ReturnsNotEmptyView()
    {
        serviceService.Get<ServiceView>(serviceView.Id).Returns(serviceView);

        object expected = NotEmptyView(controller, serviceView);
        object actual = controller.Details(serviceView.Id);

        Assert.Same(expected, actual);
    }

    #endregion

    #region Edit(String id)

    [Fact(Skip = "Need to map proper models in test")]
    public void Edit_ReturnsNotEmptyView()
    {
        serviceService.Get<ServiceView>(serviceView.Id).Returns(serviceView);

        object expected = NotEmptyView(controller, serviceView);
        object actual = controller.Edit(serviceView.Id);

        Assert.Same(expected, actual);
    }

    #endregion

    #region Edit(ServiceView service)

    [Fact]
    public void Edit_CanNotEdit_ReturnsSameView()
    {
        validator.CanEdit(serviceCreateEditView).Returns(false);

        object actual = (controller.Edit(serviceCreateEditView) as ViewResult).ViewData.Model;
        object expected = serviceCreateEditView;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Edit_Service()
    {
        validator.CanEdit(serviceCreateEditView).Returns(true);

        controller.Edit(serviceCreateEditView);

        serviceService.Received().Edit(serviceCreateEditView);
    }

    [Fact]
    public void Edit_RedirectsToIndex()
    {
        validator.CanEdit(serviceCreateEditView).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.Edit(serviceCreateEditView);

        Assert.Same(expected, actual);
    }

    #endregion

    #region Delete(String id)

    [Fact]
    public void Delete_ReturnsNotEmptyView()
    {
        serviceService.Get<ServiceView>(serviceView.Id).Returns(serviceView);

        object expected = NotEmptyView(controller, serviceView);
        object actual = controller.Delete(serviceView.Id);

        Assert.Same(expected, actual);
    }

    #endregion

    #region DeleteConfirmed(String id)

    [Fact]
    public void DeleteConfirmed_DeletesService()
    {
        controller.DeleteConfirmed(serviceView.Id);

        serviceService.Received().Delete(serviceView.Id);
    }

    [Fact]
    public void Delete_RedirectsToIndex()
    {
        object expected = RedirectToAction(controller, "Index");
        object actual = controller.DeleteConfirmed(serviceView.Id);

        Assert.Same(expected, actual);
    }

    #endregion
}
