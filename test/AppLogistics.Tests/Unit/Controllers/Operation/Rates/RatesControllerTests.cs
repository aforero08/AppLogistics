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

public class RatesControllerTests : ControllerTests
{
    private RatesController controller;
    private IRateValidator validator;
    private IRateService service;
    private RateView rateView;
    private RateCreateEditView rateCreateEditView;

    public RatesControllerTests()
    {
        validator = Substitute.For<IRateValidator>();
        validator.CanDelete(Arg.Any<int>()).Returns(true);
        service = Substitute.For<IRateService>();

        rateView = ObjectsFactory.CreateRateView();
        rateCreateEditView = ObjectsFactory.CreateRateCreateEditView();

        controller = Substitute.ForPartsOf<RatesController>(validator, service);
        controller.ControllerContext.RouteData = new RouteData();
    }

    #region Index()

    [Fact]
    public void Index_ReturnsRateViews()
    {
        service.GetViews().Returns(new RateView[0].AsQueryable());

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

    #region Create(RateView rate)

    [Fact]
    public void Create_ProtectsFromOverpostingId()
    {
        ProtectsFromOverpostingId(controller, "Create");
    }

    [Fact]
    public void Create_CanNotCreate_ReturnsSameView()
    {
        validator.CanCreate(rateCreateEditView).Returns(false);

        object actual = (controller.Create(rateCreateEditView) as ViewResult).ViewData.Model;
        object expected = rateCreateEditView;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Create_Rate()
    {
        validator.CanCreate(rateCreateEditView).Returns(true);

        controller.Create(rateCreateEditView);

        service.Received().Create(rateCreateEditView);
    }

    [Fact]
    public void Create_RedirectsToIndex()
    {
        validator.CanCreate(rateCreateEditView).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.Create(rateCreateEditView);

        Assert.Same(expected, actual);
    }

    #endregion Create(RateView rate)

    #region Details(String id)

    [Fact]
    public void Details_ReturnsNotEmptyView()
    {
        service.Get<RateView>(rateView.Id).Returns(rateView);

        object expected = NotEmptyView(controller, rateView);
        object actual = controller.Details(rateView.Id);

        Assert.Same(expected, actual);
    }

    #endregion Details(String id)

    #region Edit(String id)

    [Fact]
    public void Edit_ReturnsNotEmptyView()
    {
        service.Get<RateCreateEditView>(rateCreateEditView.Id).Returns(rateCreateEditView);

        object expected = NotEmptyView(controller, rateCreateEditView);
        object actual = controller.Edit(rateCreateEditView.Id);

        Assert.Same(expected, actual);
    }

    #endregion Edit(String id)

    #region Edit(RateView rate)

    [Fact]
    public void Edit_CanNotEdit_ReturnsSameView()
    {
        validator.CanEdit(rateCreateEditView).Returns(false);

        object actual = (controller.Edit(rateCreateEditView) as ViewResult).ViewData.Model;
        object expected = rateCreateEditView;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Edit_Rate()
    {
        validator.CanEdit(rateCreateEditView).Returns(true);

        controller.Edit(rateCreateEditView);

        service.Received().Edit(rateCreateEditView);
    }

    [Fact]
    public void Edit_RedirectsToIndex()
    {
        validator.CanEdit(rateCreateEditView).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.Edit(rateCreateEditView);

        Assert.Same(expected, actual);
    }

    #endregion Edit(RateView rate)

    #region Delete(String id)

    [Fact]
    public void Delete_ReturnsNotEmptyView()
    {
        service.Get<RateView>(rateView.Id).Returns(rateView);

        object expected = NotEmptyView(controller, rateView);
        object actual = controller.Delete(rateView.Id);

        Assert.Same(expected, actual);
    }

    #endregion Delete(String id)

    #region DeleteConfirmed(String id)

    [Fact]
    public void DeleteConfirmed_DeletesRate()
    {
        controller.DeleteConfirmed(rateView.Id);

        service.Received().Delete(rateView.Id);
    }

    [Fact]
    public void Delete_RedirectsToIndex()
    {
        object expected = RedirectToAction(controller, "Index");
        object actual = controller.DeleteConfirmed(rateView.Id);

        Assert.Same(expected, actual);
    }

    #endregion DeleteConfirmed(String id)
}
