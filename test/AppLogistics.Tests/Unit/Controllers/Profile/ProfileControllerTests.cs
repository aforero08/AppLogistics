using AppLogistics.Components.Notifications;
using AppLogistics.Components.Security;
using AppLogistics.Objects;
using AppLogistics.Resources;
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

namespace AppLogistics.Controllers.Tests;

public class ProfileControllerTests : ControllerTests
{
    private ProfileDeleteView profileDelete;
    private ProfileController controller;
    private ProfileEditView profileEdit;
    private IAccountValidator validator;
    private IAccountService service;

    public ProfileControllerTests()
    {
        validator = Substitute.For<IAccountValidator>();
        service = Substitute.For<IAccountService>();

        profileDelete = ObjectsFactory.CreateProfileDeleteView();
        profileEdit = ObjectsFactory.CreateProfileEditView();

        controller = Substitute.ForPartsOf<ProfileController>(validator, service);
        controller.ControllerContext.HttpContext = Substitute.For<HttpContext>();
        controller.TempData = Substitute.For<ITempDataDictionary>();
        controller.ControllerContext.RouteData = new RouteData();
        controller.Url = Substitute.For<IUrlHelper>();
        ReturnCurrentAccountId(controller, 1);
    }

    #region Edit()

    [Fact]
    public void Edit_NotActive_RedirectsToLogout()
    {
        service.IsActive(controller.CurrentAccountId).Returns(false);

        object expected = RedirectToAction(controller, "Logout", "Auth");
        object actual = controller.Edit();

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Edit_ReturnsProfileView()
    {
        service.Get<ProfileEditView>(controller.CurrentAccountId).Returns(profileEdit);
        service.IsActive(controller.CurrentAccountId).Returns(true);

        object actual = (controller.Edit() as ViewResult).Model;
        object expected = profileEdit;

        Assert.Same(expected, actual);
    }

    #endregion Edit()

    #region Edit(ProfileEditView profile)

    [Fact]
    public void Edit_ProtectsFromOverpostingId()
    {
        ProtectsFromOverpostingId(controller, "Edit");
    }

    [Fact]
    public void Edit_Post_NotActive_RedirectsToLogout()
    {
        service.IsActive(controller.CurrentAccountId).Returns(false);

        object expected = RedirectToAction(controller, "Logout", "Auth");
        object actual = controller.Edit(null);

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Edit_CanNotEdit_ReturnsSameView()
    {
        service.IsActive(controller.CurrentAccountId).Returns(true);
        validator.CanEdit(profileEdit).Returns(false);

        object actual = (controller.Edit(profileEdit) as ViewResult).Model;
        object expected = profileEdit;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Edit_Profile()
    {
        service.IsActive(controller.CurrentAccountId).Returns(true);
        validator.CanEdit(profileEdit).Returns(true);

        controller.Edit(profileEdit);

        service.Received().Edit(controller.User, profileEdit);
    }

    [Fact]
    public void Edit_AddsUpdatedMessage()
    {
        service.IsActive(controller.CurrentAccountId).Returns(true);
        validator.CanEdit(profileEdit).Returns(true);

        controller.Edit(profileEdit);

        Alert actual = controller.Alerts.Single();

        Assert.Equal(Message.For<AccountView>("ProfileUpdated"), actual.Message);
        Assert.Equal(AlertType.Success, actual.Type);
        Assert.Equal(4000, actual.Timeout);
    }

    [Fact]
    public void Edit_RedirectsToEdit()
    {
        validator.CanEdit(profileEdit).Returns(true);
        service.IsActive(controller.CurrentAccountId).Returns(true);

        object expected = RedirectToAction(controller, "Edit");
        object actual = controller.Edit(profileEdit);

        Assert.Equal(expected, actual);
    }

    #endregion Edit(ProfileEditView profile)

    #region Delete()

    [Fact]
    public void Delete_NotActive_RedirectsToLogout()
    {
        service.IsActive(controller.CurrentAccountId).Returns(false);

        object expected = RedirectToAction(controller, "Logout", "Auth");
        object actual = controller.Delete();

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Delete_AddsDisclaimerMessage()
    {
        service.IsActive(controller.CurrentAccountId).Returns(true);

        controller.Delete();

        Alert actual = controller.Alerts.Single();

        Assert.Equal(Message.For<AccountView>("ProfileDeleteDisclaimer"), actual.Message);
        Assert.Equal(AlertType.Warning, actual.Type);
        Assert.Equal(0, actual.Timeout);
    }

    [Fact]
    public void Delete_ReturnsEmptyView()
    {
        service.IsActive(controller.CurrentAccountId).Returns(true);

        ViewResult actual = controller.Delete() as ViewResult;

        Assert.Null(actual.Model);
    }

    #endregion Delete()

    #region DeleteConfirmed(ProfileDeleteView profile)

    [Fact]
    public void DeleteConfirmed_ProtectsFromOverpostingId()
    {
        ProtectsFromOverpostingId(controller, "DeleteConfirmed");
    }

    [Fact]
    public void DeleteConfirmed_NotActive_RedirectsToLogout()
    {
        service.IsActive(controller.CurrentAccountId).Returns(false);

        object expected = RedirectToAction(controller, "Logout", "Auth");
        object actual = controller.DeleteConfirmed(profileDelete);

        Assert.Same(expected, actual);
    }

    [Fact]
    public void DeleteConfirmed_CanNotDelete_AddsDisclaimerMessage()
    {
        service.IsActive(controller.CurrentAccountId).Returns(true);
        validator.CanDelete(profileDelete).Returns(false);

        controller.DeleteConfirmed(profileDelete);

        Alert actual = controller.Alerts.Single();

        Assert.Equal(Message.For<AccountView>("ProfileDeleteDisclaimer"), actual.Message);
        Assert.Equal(AlertType.Warning, actual.Type);
        Assert.Equal(0, actual.Timeout);
    }

    [Fact]
    public void DeleteConfirmed_CanNotDelete_ReturnsEmptyView()
    {
        service.IsActive(controller.CurrentAccountId).Returns(true);
        validator.CanDelete(profileDelete).Returns(false);

        ViewResult actual = controller.DeleteConfirmed(profileDelete) as ViewResult;

        Assert.Null(actual.Model);
    }

    [Fact]
    public void DeleteConfirmed_DeletesProfile()
    {
        service.IsActive(controller.CurrentAccountId).Returns(true);
        validator.CanDelete(profileDelete).Returns(true);

        controller.DeleteConfirmed(profileDelete);

        service.Received().Delete(controller.CurrentAccountId);
    }

    [Fact]
    public void DeleteConfirmed_RefreshesAuthorization()
    {
        IAuthorization authorization = Substitute.For<IAuthorization>();
        System.IServiceProvider services = Substitute.For<System.IServiceProvider>();
        services.GetService(typeof(IAuthorization)).Returns(authorization);
        authorization.IsGrantedFor(Arg.Any<int?>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(true);

        ProfileController profileController = new ProfileController(validator, service)
        {
            Url = Substitute.For<IUrlHelper>(),
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { RequestServices = services },
                RouteData = new RouteData()
            }
        };

        service.IsActive(0).Returns(true);
        validator.CanDelete(profileDelete).Returns(true);
        profileController.OnActionExecuting(null);

        profileController.DeleteConfirmed(profileDelete);

        authorization.Received().Refresh();
    }

    [Fact]
    public void DeleteConfirmed_RedirectsToAuthLogout()
    {
        service.IsActive(controller.CurrentAccountId).Returns(true);
        validator.CanDelete(profileDelete).Returns(true);

        object expected = RedirectToAction(controller, "Logout", "Auth");
        object actual = controller.DeleteConfirmed(profileDelete);

        Assert.Same(expected, actual);
    }

    #endregion DeleteConfirmed(ProfileDeleteView profile)
}
