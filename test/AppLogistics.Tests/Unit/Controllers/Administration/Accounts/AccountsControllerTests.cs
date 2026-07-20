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

public class AccountsControllerTests : ControllerTests
{
    private AccountCreateView accountCreate;
    private AccountsController controller;
    private IAccountValidator validator;
    private AccountEditView accountEdit;
    private IAccountService service;
    private AccountView account;

    public AccountsControllerTests()
    {
        validator = Substitute.For<IAccountValidator>();
        service = Substitute.For<IAccountService>();

        accountCreate = ObjectsFactory.CreateAccountCreateView();
        accountEdit = ObjectsFactory.CreateAccountEditView();
        account = ObjectsFactory.CreateAccountView();

        controller = Substitute.ForPartsOf<AccountsController>(validator, service);
        controller.ControllerContext.HttpContext = Substitute.For<HttpContext>();
        controller.TempData = Substitute.For<ITempDataDictionary>();
        controller.ControllerContext.RouteData = new RouteData();
        controller.Url = Substitute.For<IUrlHelper>();
    }

    #region Index()

    [Fact]
    public void Index_ReturnsAccountViews()
    {
        service.GetViews().Returns(new AccountView[0].AsQueryable());

        object actual = controller.Index().Model;
        object expected = service.GetViews();

        Assert.Same(expected, actual);
    }

    #endregion Index()

    #region Create()

    [Fact]
    public void Create_ReturnsEmptyView()
    {
        ViewResult actual = controller.Create();

        Assert.Null(actual.Model);
    }

    #endregion Create()

    #region Create(AccountCreateView account)

    [Fact]
    public void Create_ProtectsFromOverpostingId()
    {
        ProtectsFromOverpostingId(controller, "Create");
    }

    [Fact]
    public void Create_CanNotCreate_ReturnsSameView()
    {
        validator.CanCreate(accountCreate).Returns(false);

        object actual = (controller.Create(accountCreate) as ViewResult).Model;
        object expected = accountCreate;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Create_Account()
    {
        validator.CanCreate(accountCreate).Returns(true);

        controller.Create(accountCreate);

        service.Received().Create(accountCreate);
    }

    [Fact]
    public void Create_RefreshesAuthorization()
    {
        controller.HttpContext.RequestServices.GetService(typeof(IAuthorization)).Returns(Substitute.For<IAuthorization>());
        validator.CanCreate(accountCreate).Returns(true);
        controller.OnActionExecuting(null);

        controller.Create(accountCreate);

        controller.Authorization.Received().Refresh();
    }

    [Fact]
    public void Create_RedirectsToIndex()
    {
        validator.CanCreate(accountCreate).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.Create(accountCreate);

        Assert.Same(expected, actual);
    }

    #endregion Create(AccountCreateView account)

    #region Details(Int32 id)

    [Fact]
    public void Details_ReturnsNotEmptyView()
    {
        service.Get<AccountView>(account.Id).Returns(account);

        object expected = NotEmptyView(controller, account);
        object actual = controller.Details(account.Id);

        Assert.Same(expected, actual);
    }

    #endregion Details(Int32 id)

    #region Edit(Int32 id)

    [Fact]
    public void Edit_ReturnsNotEmptyView()
    {
        service.Get<AccountEditView>(accountEdit.Id).Returns(accountEdit);

        object expected = NotEmptyView(controller, accountEdit);
        object actual = controller.Edit(accountEdit.Id);

        Assert.Same(expected, actual);
    }

    #endregion Edit(Int32 id)

    #region Edit(AccountEditView account)

    [Fact]
    public void Edit_CanNotEdit_ReturnsSameView()
    {
        validator.CanEdit(accountEdit).Returns(false);

        object actual = (controller.Edit(accountEdit) as ViewResult).Model;
        object expected = accountEdit;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Edit_Account()
    {
        validator.CanEdit(accountEdit).Returns(true);

        controller.Edit(accountEdit);

        service.Received().Edit(accountEdit);
    }

    [Fact]
    public void Edit_RefreshesAuthorization()
    {
        controller.HttpContext.RequestServices.GetService(typeof(IAuthorization)).Returns(Substitute.For<IAuthorization>());
        validator.CanEdit(accountEdit).Returns(true);
        controller.OnActionExecuting(null);

        controller.Edit(accountEdit);

        controller.Authorization.Received().Refresh();
    }

    [Fact]
    public void Edit_RedirectsToIndex()
    {
        validator.CanEdit(accountEdit).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.Edit(accountEdit);

        Assert.Same(expected, actual);
    }

    #endregion Edit(AccountEditView account)
}
