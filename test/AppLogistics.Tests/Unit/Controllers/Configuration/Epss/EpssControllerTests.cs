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

public class EpssControllerTests : ControllerTests
{
    private EpssController controller;
    private IEpsValidator validator;
    private IEpsService service;
    private EpsView eps;

    public EpssControllerTests()
    {
        validator = Substitute.For<IEpsValidator>();
        validator.CanDelete(Arg.Any<int>()).Returns(true);
        service = Substitute.For<IEpsService>();

        eps = ObjectsFactory.CreateEpsView();

        controller = Substitute.ForPartsOf<EpssController>(validator, service);
        controller.ControllerContext.RouteData = new RouteData();
    }

    #region Index()

    [Fact]
    public void Index_ReturnsEpsViews()
    {
        service.GetViews().Returns(new EpsView[0].AsQueryable());

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

    #region Create(EpsView eps)

    [Fact]
    public void Create_ProtectsFromOverpostingId()
    {
        ProtectsFromOverpostingId(controller, "Create");
    }

    [Fact]
    public void Create_CanNotCreate_ReturnsSameView()
    {
        validator.CanCreate(eps).Returns(false);

        object actual = (controller.Create(eps) as ViewResult).ViewData.Model;
        object expected = eps;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Create_Eps()
    {
        validator.CanCreate(eps).Returns(true);

        controller.Create(eps);

        service.Received().Create(eps);
    }

    [Fact]
    public void Create_RedirectsToIndex()
    {
        validator.CanCreate(eps).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.Create(eps);

        Assert.Same(expected, actual);
    }

    #endregion Create(EpsView eps)

    #region Details(String id)

    [Fact]
    public void Details_ReturnsNotEmptyView()
    {
        service.Get<EpsView>(eps.Id).Returns(eps);

        object expected = NotEmptyView(controller, eps);
        object actual = controller.Details(eps.Id);

        Assert.Same(expected, actual);
    }

    #endregion Details(String id)

    #region Edit(String id)

    [Fact]
    public void Edit_ReturnsNotEmptyView()
    {
        service.Get<EpsView>(eps.Id).Returns(eps);

        object expected = NotEmptyView(controller, eps);
        object actual = controller.Edit(eps.Id);

        Assert.Same(expected, actual);
    }

    #endregion Edit(String id)

    #region Edit(EpsView eps)

    [Fact]
    public void Edit_CanNotEdit_ReturnsSameView()
    {
        validator.CanEdit(eps).Returns(false);

        object actual = (controller.Edit(eps) as ViewResult).ViewData.Model;
        object expected = eps;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Edit_Eps()
    {
        validator.CanEdit(eps).Returns(true);

        controller.Edit(eps);

        service.Received().Edit(eps);
    }

    [Fact]
    public void Edit_RedirectsToIndex()
    {
        validator.CanEdit(eps).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.Edit(eps);

        Assert.Same(expected, actual);
    }

    #endregion Edit(EpsView eps)

    #region Delete(String id)

    [Fact]
    public void Delete_ReturnsNotEmptyView()
    {
        service.Get<EpsView>(eps.Id).Returns(eps);

        object expected = NotEmptyView(controller, eps);
        object actual = controller.Delete(eps.Id);

        Assert.Same(expected, actual);
    }

    #endregion Delete(String id)

    #region DeleteConfirmed(String id)

    [Fact]
    public void DeleteConfirmed_DeletesEps()
    {
        controller.DeleteConfirmed(eps.Id);

        service.Received().Delete(eps.Id);
    }

    [Fact]
    public void Delete_RedirectsToIndex()
    {
        object expected = RedirectToAction(controller, "Index");
        object actual = controller.DeleteConfirmed(eps.Id);

        Assert.Same(expected, actual);
    }

    #endregion DeleteConfirmed(String id)
}
