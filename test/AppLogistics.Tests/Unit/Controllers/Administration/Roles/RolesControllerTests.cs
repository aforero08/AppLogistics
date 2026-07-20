using AppLogistics.Components.Security;
using AppLogistics.Controllers.Tests;
using AppLogistics.Objects;
using AppLogistics.Services;
using AppLogistics.Tests;
using AppLogistics.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using NSubstitute;
using System.Linq;
using Xunit;

namespace AppLogistics.Controllers.Administration.Tests;

public class RolesControllerTests : ControllerTests
{
    private RolesController controller;
    private IRoleValidator validator;
    private IRoleService service;
    private RoleView role;

    public RolesControllerTests()
    {
        validator = Substitute.For<IRoleValidator>();
        service = Substitute.For<IRoleService>();

        role = ObjectsFactory.CreateRoleView();

        controller = Substitute.ForPartsOf<RolesController>(validator, service);
        controller.ControllerContext.HttpContext = Substitute.For<HttpContext>();
        controller.TempData = Substitute.For<ITempDataDictionary>();
        controller.ControllerContext.RouteData = new RouteData();
        controller.Url = Substitute.For<IUrlHelper>();
    }

    #region Index()

    [Fact]
    public void Index_ReturnsRoleViews()
    {
        service.GetViews().Returns(new RoleView[0].AsQueryable());

        object actual = controller.Index().Model;
        object expected = service.GetViews();

        Assert.Same(expected, actual);
    }

    #endregion Index()

    #region Create()

    [Fact]
    public void Create_ReturnsNewRoleView()
    {
        RoleView actual = controller.Create().Model as RoleView;

        Assert.NotNull(actual.Permissions);
        Assert.Null(actual.Title);
    }

    [Fact]
    public void Create_SeedsPermissions()
    {
        RoleView view = controller.Create().Model as RoleView;

        service.Received().SeedPermissions(view);
    }

    #endregion Create()

    #region Create(RoleView role)

    [Fact]
    public void Create_ProtectsFromOverpostingId()
    {
        ProtectsFromOverpostingId(controller, "Create");
    }

    [Fact]
    public void Create_CanNotCreate_SeedsPermissions()
    {
        validator.CanCreate(role).Returns(false);

        controller.Create(role);

        service.Received().SeedPermissions(role);
    }

    [Fact]
    public void Create_CanNotCreate_ReturnsSameView()
    {
        validator.CanCreate(role).Returns(false);

        object actual = (controller.Create(role) as ViewResult).Model;
        object expected = role;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Create_Role()
    {
        validator.CanCreate(role).Returns(true);

        controller.Create(role);

        service.Received().Create(role);
    }

    [Fact]
    public void Create_RedirectsToIndex()
    {
        validator.CanCreate(role).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.Create(role);

        Assert.Same(expected, actual);
    }

    #endregion Create(RoleView role)

    #region Details(Int32 id)

    [Fact]
    public void Details_ReturnsNotEmptyView()
    {
        service.GetView(role.Id).Returns(role);

        object expected = NotEmptyView(controller, role);
        object actual = controller.Details(role.Id);

        Assert.Same(expected, actual);
    }

    #endregion Details(Int32 id)

    #region Edit(Int32 id)

    [Fact]
    public void Edit_ReturnsNotEmptyView()
    {
        service.GetView(role.Id).Returns(role);

        object expected = NotEmptyView(controller, role);
        object actual = controller.Edit(role.Id);

        Assert.Same(expected, actual);
    }

    #endregion Edit(Int32 id)

    #region Edit(RoleView role)

    [Fact]
    public void Edit_CanNotEdit_SeedsPermissions()
    {
        validator.CanEdit(role).Returns(false);

        controller.Edit(role);

        service.Received().SeedPermissions(role);
    }

    [Fact]
    public void Edit_CanNotEdit_ReturnsSameView()
    {
        validator.CanEdit(role).Returns(false);

        object actual = (controller.Edit(role) as ViewResult).Model;
        object expected = role;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Edit_Role()
    {
        validator.CanEdit(role).Returns(true);

        controller.Edit(role);

        service.Received().Edit(role);
    }

    [Fact]
    public void Edit_RefreshesAuthorization()
    {
        controller.HttpContext.RequestServices.GetService(typeof(IAuthorization)).Returns(Substitute.For<IAuthorization>());
        validator.CanEdit(role).Returns(true);
        controller.OnActionExecuting(null);

        controller.Edit(role);

        controller.Authorization.Received().Refresh();
    }

    [Fact]
    public void Edit_RedirectsToIndex()
    {
        validator.CanEdit(role).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.Edit(role);

        Assert.Same(expected, actual);
    }

    #endregion Edit(RoleView role)

    #region Delete(Int32 id)

    [Fact]
    public void Delete_ReturnsNotEmptyView()
    {
        service.GetView(role.Id).Returns(role);

        object expected = NotEmptyView(controller, role);
        object actual = controller.Delete(role.Id);

        Assert.Same(expected, actual);
    }

    #endregion Delete(Int32 id)

    #region DeleteConfirmed(Int32 id)

    [Fact]
    public void DeleteConfirmed_DeletesRole()
    {
        controller.DeleteConfirmed(role.Id);

        service.Received().Delete(role.Id);
    }

    [Fact]
    public void DeleteConfirmed_RefreshesAuthorization()
    {
        controller.HttpContext.RequestServices.GetService(typeof(IAuthorization)).Returns(Substitute.For<IAuthorization>());
        controller.OnActionExecuting(null);

        controller.DeleteConfirmed(role.Id);

        controller.Authorization.Received().Refresh();
    }

    [Fact]
    public void DeleteConfirmed_RedirectsToIndex()
    {
        object expected = RedirectToAction(controller, "Index");
        object actual = controller.DeleteConfirmed(role.Id);

        Assert.Same(expected, actual);
    }

    #endregion DeleteConfirmed(Int32 id)
}
