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

public class SexesControllerTests : ControllerTests
{
    private SexesController controller;
    private ISexValidator validator;
    private ISexService service;
    private SexView sex;

    public SexesControllerTests()
    {
        validator = Substitute.For<ISexValidator>();
        validator.CanDelete(Arg.Any<int>()).Returns(true);
        service = Substitute.For<ISexService>();

        sex = ObjectsFactory.CreateSexView();

        controller = Substitute.ForPartsOf<SexesController>(validator, service);
        controller.ControllerContext.RouteData = new RouteData();
    }

    #region Index()

    [Fact]
    public void Index_ReturnsSexViews()
    {
        service.GetViews().Returns(new SexView[0].AsQueryable());

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

    #region Create(SexView sex)

    [Fact]
    public void Create_ProtectsFromOverpostingId()
    {
        ProtectsFromOverpostingId(controller, "Create");
    }

    [Fact]
    public void Create_CanNotCreate_ReturnsSameView()
    {
        validator.CanCreate(sex).Returns(false);

        object actual = (controller.Create(sex) as ViewResult).ViewData.Model;
        object expected = sex;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Create_Sex()
    {
        validator.CanCreate(sex).Returns(true);

        controller.Create(sex);

        service.Received().Create(sex);
    }

    [Fact]
    public void Create_RedirectsToIndex()
    {
        validator.CanCreate(sex).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.Create(sex);

        Assert.Same(expected, actual);
    }

    #endregion

    #region Details(String id)

    [Fact]
    public void Details_ReturnsNotEmptyView()
    {
        service.Get<SexView>(sex.Id).Returns(sex);

        object expected = NotEmptyView(controller, sex);
        object actual = controller.Details(sex.Id);

        Assert.Same(expected, actual);
    }

    #endregion

    #region Edit(String id)

    [Fact]
    public void Edit_ReturnsNotEmptyView()
    {
        service.Get<SexView>(sex.Id).Returns(sex);

        object expected = NotEmptyView(controller, sex);
        object actual = controller.Edit(sex.Id);

        Assert.Same(expected, actual);
    }

    #endregion

    #region Edit(SexView sex)

    [Fact]
    public void Edit_CanNotEdit_ReturnsSameView()
    {
        validator.CanEdit(sex).Returns(false);

        object actual = (controller.Edit(sex) as ViewResult).ViewData.Model;
        object expected = sex;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Edit_Sex()
    {
        validator.CanEdit(sex).Returns(true);

        controller.Edit(sex);

        service.Received().Edit(sex);
    }

    [Fact]
    public void Edit_RedirectsToIndex()
    {
        validator.CanEdit(sex).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.Edit(sex);

        Assert.Same(expected, actual);
    }

    #endregion

    #region Delete(String id)

    [Fact]
    public void Delete_ReturnsNotEmptyView()
    {
        service.Get<SexView>(sex.Id).Returns(sex);

        object expected = NotEmptyView(controller, sex);
        object actual = controller.Delete(sex.Id);

        Assert.Same(expected, actual);
    }

    #endregion

    #region DeleteConfirmed(String id)

    [Fact]
    public void DeleteConfirmed_DeletesSex()
    {
        controller.DeleteConfirmed(sex.Id);

        service.Received().Delete(sex.Id);
    }

    [Fact]
    public void Delete_RedirectsToIndex()
    {
        object expected = RedirectToAction(controller, "Index");
        object actual = controller.DeleteConfirmed(sex.Id);

        Assert.Same(expected, actual);
    }

    #endregion
}
