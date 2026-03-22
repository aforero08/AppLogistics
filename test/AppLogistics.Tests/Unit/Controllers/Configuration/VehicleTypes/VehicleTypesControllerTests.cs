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

namespace AppLogistics.Controllers.Configuration.Tests;

public class VehicleTypesControllerTests : ControllerTests
{
    private VehicleTypesController controller;
    private IVehicleTypeValidator validator;
    private IVehicleTypeService service;
    private VehicleTypeView vehicleType;

    public VehicleTypesControllerTests()
    {
        validator = Substitute.For<IVehicleTypeValidator>();
        validator.CanDelete(Arg.Any<int>()).Returns(true);
        service = Substitute.For<IVehicleTypeService>();

        vehicleType = ObjectsFactory.CreateVehicleTypeView();

        controller = Substitute.ForPartsOf<VehicleTypesController>(validator, service);
        controller.ControllerContext.RouteData = new RouteData();
    }

    #region Index()

    [Fact]
    public void Index_ReturnsVehicleTypeViews()
    {
        service.GetViews().Returns(new VehicleTypeView[0].AsQueryable());

        object actual = controller.Index().ViewData.Model;
        object expected = service.GetViews();

        Assert.Same(expected, actual);
    }

    #endregion Index()

    #region Create()

    [Fact]
    public void Create_ReturnsEmptyView()
    {
        ViewDataDictionary actual = controller.Create().ViewData;

        Assert.Null(actual.Model);
    }

    #endregion Create()

    #region Create(VehicleTypeView vehicleType)

    [Fact]
    public void Create_ProtectsFromOverpostingId()
    {
        ProtectsFromOverpostingId(controller, "Create");
    }

    [Fact]
    public void Create_CanNotCreate_ReturnsSameView()
    {
        validator.CanCreate(vehicleType).Returns(false);

        object actual = (controller.Create(vehicleType) as ViewResult).ViewData.Model;
        object expected = vehicleType;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Create_VehicleType()
    {
        validator.CanCreate(vehicleType).Returns(true);

        controller.Create(vehicleType);

        service.Received().Create(vehicleType);
    }

    [Fact]
    public void Create_RedirectsToIndex()
    {
        validator.CanCreate(vehicleType).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.Create(vehicleType);

        Assert.Same(expected, actual);
    }

    #endregion Create(VehicleTypeView vehicleType)

    #region Details(String id)

    [Fact]
    public void Details_ReturnsNotEmptyView()
    {
        service.Get<VehicleTypeView>(vehicleType.Id).Returns(vehicleType);

        object expected = NotEmptyView(controller, vehicleType);
        object actual = controller.Details(vehicleType.Id);

        Assert.Same(expected, actual);
    }

    #endregion Details(String id)

    #region Edit(String id)

    [Fact]
    public void Edit_ReturnsNotEmptyView()
    {
        service.Get<VehicleTypeView>(vehicleType.Id).Returns(vehicleType);

        object expected = NotEmptyView(controller, vehicleType);
        object actual = controller.Edit(vehicleType.Id);

        Assert.Same(expected, actual);
    }

    #endregion Edit(String id)

    #region Edit(VehicleTypeView vehicleType)

    [Fact]
    public void Edit_CanNotEdit_ReturnsSameView()
    {
        validator.CanEdit(vehicleType).Returns(false);

        object actual = (controller.Edit(vehicleType) as ViewResult).ViewData.Model;
        object expected = vehicleType;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Edit_VehicleType()
    {
        validator.CanEdit(vehicleType).Returns(true);

        controller.Edit(vehicleType);

        service.Received().Edit(vehicleType);
    }

    [Fact]
    public void Edit_RedirectsToIndex()
    {
        validator.CanEdit(vehicleType).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.Edit(vehicleType);

        Assert.Same(expected, actual);
    }

    #endregion Edit(VehicleTypeView vehicleType)

    #region Delete(String id)

    [Fact]
    public void Delete_ReturnsNotEmptyView()
    {
        service.Get<VehicleTypeView>(vehicleType.Id).Returns(vehicleType);

        object expected = NotEmptyView(controller, vehicleType);
        object actual = controller.Delete(vehicleType.Id);

        Assert.Same(expected, actual);
    }

    #endregion Delete(String id)

    #region DeleteConfirmed(String id)

    [Fact]
    public void DeleteConfirmed_DeletesVehicleType()
    {
        controller.DeleteConfirmed(vehicleType.Id);

        service.Received().Delete(vehicleType.Id);
    }

    [Fact]
    public void Delete_RedirectsToIndex()
    {
        object expected = RedirectToAction(controller, "Index");
        object actual = controller.DeleteConfirmed(vehicleType.Id);

        Assert.Same(expected, actual);
    }

    #endregion DeleteConfirmed(String id)
}
