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

public class CarriersControllerTests : ControllerTests
{
    private CarriersController controller;
    private ICarrierValidator validator;
    private ICarrierService service;
    private CarrierView carrier;

    public CarriersControllerTests()
    {
        validator = Substitute.For<ICarrierValidator>();
        validator.CanDelete(Arg.Any<int>()).Returns(true);
        service = Substitute.For<ICarrierService>();

        carrier = ObjectsFactory.CreateCarrierView();

        controller = Substitute.ForPartsOf<CarriersController>(validator, service);
        controller.ControllerContext.RouteData = new RouteData();
    }

    #region Index()

    [Fact]
    public void Index_ReturnsCarrierViews()
    {
        service.GetViews().Returns(new CarrierView[0].AsQueryable());

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

    #region Create(CarrierView carrier)

    [Fact]
    public void Create_ProtectsFromOverpostingId()
    {
        ProtectsFromOverpostingId(controller, "Create");
    }

    [Fact]
    public void Create_CanNotCreate_ReturnsSameView()
    {
        validator.CanCreate(carrier).Returns(false);

        object actual = (controller.Create(carrier) as ViewResult).ViewData.Model;
        object expected = carrier;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Create_Carrier()
    {
        validator.CanCreate(carrier).Returns(true);

        controller.Create(carrier);

        service.Received().Create(carrier);
    }

    [Fact]
    public void Create_RedirectsToIndex()
    {
        validator.CanCreate(carrier).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.Create(carrier);

        Assert.Same(expected, actual);
    }

    #endregion Create(CarrierView carrier)

    #region Details(String id)

    [Fact]
    public void Details_ReturnsNotEmptyView()
    {
        service.Get<CarrierView>(carrier.Id).Returns(carrier);

        object expected = NotEmptyView(controller, carrier);
        object actual = controller.Details(carrier.Id);

        Assert.Same(expected, actual);
    }

    #endregion Details(String id)

    #region Edit(String id)

    [Fact]
    public void Edit_ReturnsNotEmptyView()
    {
        service.Get<CarrierView>(carrier.Id).Returns(carrier);

        object expected = NotEmptyView(controller, carrier);
        object actual = controller.Edit(carrier.Id);

        Assert.Same(expected, actual);
    }

    #endregion Edit(String id)

    #region Edit(CarrierView carrier)

    [Fact]
    public void Edit_CanNotEdit_ReturnsSameView()
    {
        validator.CanEdit(carrier).Returns(false);

        object actual = (controller.Edit(carrier) as ViewResult).ViewData.Model;
        object expected = carrier;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Edit_Carrier()
    {
        validator.CanEdit(carrier).Returns(true);

        controller.Edit(carrier);

        service.Received().Edit(carrier);
    }

    [Fact]
    public void Edit_RedirectsToIndex()
    {
        validator.CanEdit(carrier).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.Edit(carrier);

        Assert.Same(expected, actual);
    }

    #endregion Edit(CarrierView carrier)

    #region Delete(String id)

    [Fact]
    public void Delete_ReturnsNotEmptyView()
    {
        service.Get<CarrierView>(carrier.Id).Returns(carrier);

        object expected = NotEmptyView(controller, carrier);
        object actual = controller.Delete(carrier.Id);

        Assert.Same(expected, actual);
    }

    #endregion Delete(String id)

    #region DeleteConfirmed(String id)

    [Fact]
    public void DeleteConfirmed_DeletesCarrier()
    {
        controller.DeleteConfirmed(carrier.Id);

        service.Received().Delete(carrier.Id);
    }

    [Fact]
    public void Delete_RedirectsToIndex()
    {
        object expected = RedirectToAction(controller, "Index");
        object actual = controller.DeleteConfirmed(carrier.Id);

        Assert.Same(expected, actual);
    }

    #endregion DeleteConfirmed(String id)
}
