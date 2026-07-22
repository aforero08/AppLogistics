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

public class AfpsControllerTests : ControllerTests
{
    private AfpsController controller;
    private IAfpValidator validator;
    private IAfpService service;
    private AfpView afp;

    public AfpsControllerTests()
    {
        validator = Substitute.For<IAfpValidator>();
        validator.CanDelete(Arg.Any<int>()).Returns(true);
        service = Substitute.For<IAfpService>();

        afp = ObjectsFactory.CreateAfpView();

        controller = Substitute.ForPartsOf<AfpsController>(validator, service);
        controller.ControllerContext.RouteData = new RouteData();
    }

    #region Index()

    [Fact]
    public void Index_ReturnsAfpViews()
    {
        service.GetViews().Returns(new AfpView[0].AsQueryable());

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

    #region Create(AfpView afp)

    [Fact]
    public void Create_ProtectsFromOverpostingId()
    {
        ProtectsFromOverpostingId(controller, "Create");
    }

    [Fact]
    public void Create_CanNotCreate_ReturnsSameView()
    {
        validator.CanCreate(afp).Returns(false);

        object actual = (controller.Create(afp) as ViewResult).ViewData.Model;
        object expected = afp;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Create_Afp()
    {
        validator.CanCreate(afp).Returns(true);

        controller.Create(afp);

        service.Received().Create(afp);
    }

    [Fact]
    public void Create_RedirectsToIndex()
    {
        validator.CanCreate(afp).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.Create(afp);

        Assert.Same(expected, actual);
    }

    #endregion Create(AfpView afp)

    #region Details(String id)

    [Fact]
    public void Details_ReturnsNotEmptyView()
    {
        service.Get<AfpView>(afp.Id).Returns(afp);

        object expected = NotEmptyView(controller, afp);
        object actual = controller.Details(afp.Id);

        Assert.Same(expected, actual);
    }

    #endregion Details(String id)

    #region Edit(String id)

    [Fact]
    public void Edit_ReturnsNotEmptyView()
    {
        service.Get<AfpView>(afp.Id).Returns(afp);

        object expected = NotEmptyView(controller, afp);
        object actual = controller.Edit(afp.Id);

        Assert.Same(expected, actual);
    }

    #endregion Edit(String id)

    #region Edit(AfpView afp)

    [Fact]
    public void Edit_CanNotEdit_ReturnsSameView()
    {
        validator.CanEdit(afp).Returns(false);

        object actual = (controller.Edit(afp) as ViewResult).ViewData.Model;
        object expected = afp;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Edit_Afp()
    {
        validator.CanEdit(afp).Returns(true);

        controller.Edit(afp);

        service.Received().Edit(afp);
    }

    [Fact]
    public void Edit_RedirectsToIndex()
    {
        validator.CanEdit(afp).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.Edit(afp);

        Assert.Same(expected, actual);
    }

    #endregion Edit(AfpView afp)

    #region Delete(String id)

    [Fact]
    public void Delete_ReturnsNotEmptyView()
    {
        service.Get<AfpView>(afp.Id).Returns(afp);

        object expected = NotEmptyView(controller, afp);
        object actual = controller.Delete(afp.Id);

        Assert.Same(expected, actual);
    }

    #endregion Delete(String id)

    #region DeleteConfirmed(String id)

    [Fact]
    public void DeleteConfirmed_DeletesAfp()
    {
        controller.DeleteConfirmed(afp.Id);

        service.Received().Delete(afp.Id);
    }

    [Fact]
    public void Delete_RedirectsToIndex()
    {
        object expected = RedirectToAction(controller, "Index");
        object actual = controller.DeleteConfirmed(afp.Id);

        Assert.Same(expected, actual);
    }

    #endregion DeleteConfirmed(String id)
}
