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

public class SectorsControllerTests : ControllerTests
{
    private SectorsController controller;
    private ISectorValidator validator;
    private ISectorService service;
    private SectorView sector;

    public SectorsControllerTests()
    {
        validator = Substitute.For<ISectorValidator>();
        validator.CanDelete(Arg.Any<int>()).Returns(true);
        service = Substitute.For<ISectorService>();

        sector = ObjectsFactory.CreateSectorView();

        controller = Substitute.ForPartsOf<SectorsController>(validator, service);
        controller.ControllerContext.RouteData = new RouteData();
    }

    #region Index()

    [Fact]
    public void Index_ReturnsSectorViews()
    {
        service.GetViews().Returns(new SectorView[0].AsQueryable());

        object actual = controller.Index().ViewData.Model;
        object expected = service.GetViews();

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

    #region Create(SectorView sector)

    [Fact]
    public void Create_ProtectsFromOverpostingId()
    {
        ProtectsFromOverpostingId(controller, "Create");
    }

    [Fact]
    public void Create_CanNotCreate_ReturnsSameView()
    {
        validator.CanCreate(sector).Returns(false);

        object actual = (controller.Create(sector) as ViewResult).ViewData.Model;
        object expected = sector;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Create_Sector()
    {
        validator.CanCreate(sector).Returns(true);

        controller.Create(sector);

        service.Received().Create(sector);
    }

    [Fact]
    public void Create_RedirectsToIndex()
    {
        validator.CanCreate(sector).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.Create(sector);

        Assert.Same(expected, actual);
    }

    #endregion

    #region Details(String id)

    [Fact]
    public void Details_ReturnsNotEmptyView()
    {
        service.Get<SectorView>(sector.Id).Returns(sector);

        object expected = NotEmptyView(controller, sector);
        object actual = controller.Details(sector.Id);

        Assert.Same(expected, actual);
    }

    #endregion

    #region Edit(String id)

    [Fact]
    public void Edit_ReturnsNotEmptyView()
    {
        service.Get<SectorView>(sector.Id).Returns(sector);

        object expected = NotEmptyView(controller, sector);
        object actual = controller.Edit(sector.Id);

        Assert.Same(expected, actual);
    }

    #endregion

    #region Edit(SectorView sector)

    [Fact]
    public void Edit_CanNotEdit_ReturnsSameView()
    {
        validator.CanEdit(sector).Returns(false);

        object actual = (controller.Edit(sector) as ViewResult).ViewData.Model;
        object expected = sector;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Edit_Sector()
    {
        validator.CanEdit(sector).Returns(true);

        controller.Edit(sector);

        service.Received().Edit(sector);
    }

    [Fact]
    public void Edit_RedirectsToIndex()
    {
        validator.CanEdit(sector).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.Edit(sector);

        Assert.Same(expected, actual);
    }

    #endregion

    #region Delete(String id)

    [Fact]
    public void Delete_ReturnsNotEmptyView()
    {
        service.Get<SectorView>(sector.Id).Returns(sector);

        object expected = NotEmptyView(controller, sector);
        object actual = controller.Delete(sector.Id);

        Assert.Same(expected, actual);
    }

    #endregion

    #region DeleteConfirmed(String id)

    [Fact]
    public void DeleteConfirmed_DeletesSector()
    {
        controller.DeleteConfirmed(sector.Id);

        service.Received().Delete(sector.Id);
    }

    [Fact]
    public void Delete_RedirectsToIndex()
    {
        object expected = RedirectToAction(controller, "Index");
        object actual = controller.DeleteConfirmed(sector.Id);

        Assert.Same(expected, actual);
    }

    #endregion
}
