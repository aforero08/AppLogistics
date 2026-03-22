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

public class ClientsControllerTests : ControllerTests
{
    private ClientsController controller;
    private IClientValidator validator;
    private IClientService service;
    private ClientView clientView;
    private ClientCreateEditView clientCreateEditView;

    public ClientsControllerTests()
    {
        validator = Substitute.For<IClientValidator>();
        validator.CanDelete(Arg.Any<int>()).Returns(true);
        service = Substitute.For<IClientService>();

        clientView = ObjectsFactory.CreateClientView();
        clientCreateEditView = ObjectsFactory.CreateClientCreateEditView();

        controller = Substitute.ForPartsOf<ClientsController>(validator, service);
        controller.ControllerContext.RouteData = new RouteData();
    }

    #region Index()

    [Fact]
    public void Index_ReturnsClientViews()
    {
        service.GetViews().Returns(new ClientView[0].AsQueryable());

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

    #region Create(ClientView client)

    [Fact]
    public void Create_ProtectsFromOverpostingId()
    {
        ProtectsFromOverpostingId(controller, "Create");
    }

    [Fact]
    public void Create_CanNotCreate_ReturnsSameView()
    {
        validator.CanCreate(clientCreateEditView).Returns(false);

        object actual = (controller.Create(clientCreateEditView) as ViewResult).ViewData.Model;
        object expected = clientCreateEditView;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Create_Client()
    {
        validator.CanCreate(clientCreateEditView).Returns(true);

        controller.Create(clientCreateEditView);

        service.Received().Create(clientCreateEditView);
    }

    [Fact]
    public void Create_RedirectsToIndex()
    {
        validator.CanCreate(clientCreateEditView).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.Create(clientCreateEditView);

        Assert.Same(expected, actual);
    }

    #endregion Create(ClientView client)

    #region Details(String id)

    [Fact]
    public void Details_ReturnsNotEmptyView()
    {
        service.Get<ClientView>(clientView.Id).Returns(clientView);

        object expected = NotEmptyView(controller, clientView);
        object actual = controller.Details(clientView.Id);

        Assert.Same(expected, actual);
    }

    #endregion Details(String id)

    #region Edit(String id)

    [Fact]
    public void Edit_ReturnsNotEmptyView()
    {
        service.Get<ClientCreateEditView>(clientCreateEditView.Id).Returns(clientCreateEditView);

        object expected = NotEmptyView(controller, clientCreateEditView);
        object actual = controller.Edit(clientCreateEditView.Id);

        Assert.Same(expected, actual);
    }

    #endregion Edit(String id)

    #region Edit(ClientView client)

    [Fact]
    public void Edit_CanNotEdit_ReturnsSameView()
    {
        validator.CanEdit(clientCreateEditView).Returns(false);

        object actual = (controller.Edit(clientCreateEditView) as ViewResult).ViewData.Model;
        object expected = clientCreateEditView;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Edit_Client()
    {
        validator.CanEdit(clientCreateEditView).Returns(true);

        controller.Edit(clientCreateEditView);

        service.Received().Edit(clientCreateEditView);
    }

    [Fact]
    public void Edit_RedirectsToIndex()
    {
        validator.CanEdit(clientCreateEditView).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.Edit(clientCreateEditView);

        Assert.Same(expected, actual);
    }

    #endregion Edit(ClientView client)

    #region Delete(String id)

    [Fact]
    public void Delete_ReturnsNotEmptyView()
    {
        service.Get<ClientView>(clientView.Id).Returns(clientView);

        object expected = NotEmptyView(controller, clientView);
        object actual = controller.Delete(clientView.Id);

        Assert.Same(expected, actual);
    }

    #endregion Delete(String id)

    #region DeleteConfirmed(String id)

    [Fact]
    public void DeleteConfirmed_DeletesClient()
    {
        controller.DeleteConfirmed(clientView.Id);

        service.Received().Delete(clientView.Id);
    }

    [Fact]
    public void Delete_RedirectsToIndex()
    {
        object expected = RedirectToAction(controller, "Index");
        object actual = controller.DeleteConfirmed(clientView.Id);

        Assert.Same(expected, actual);
    }

    #endregion DeleteConfirmed(String id)
}
