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

public class MaritalStatusesControllerTests : ControllerTests
{
    private MaritalStatusesController controller;
    private IMaritalStatusValidator validator;
    private IMaritalStatusService service;
    private MaritalStatusView maritalStatus;

    public MaritalStatusesControllerTests()
    {
        validator = Substitute.For<IMaritalStatusValidator>();
        validator.CanDelete(Arg.Any<int>()).Returns(true);
        service = Substitute.For<IMaritalStatusService>();

        maritalStatus = ObjectsFactory.CreateMaritalStatusView();

        controller = Substitute.ForPartsOf<MaritalStatusesController>(validator, service);
        controller.ControllerContext.RouteData = new RouteData();
    }

    #region Index()

    [Fact]
    public void Index_ReturnsMaritalStatusViews()
    {
        service.GetViews().Returns(new MaritalStatusView[0].AsQueryable());

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

    #region Create(MaritalStatusView maritalStatus)

    [Fact]
    public void Create_ProtectsFromOverpostingId()
    {
        ProtectsFromOverpostingId(controller, "Create");
    }

    [Fact]
    public void Create_CanNotCreate_ReturnsSameView()
    {
        validator.CanCreate(maritalStatus).Returns(false);

        object actual = (controller.Create(maritalStatus) as ViewResult).ViewData.Model;
        object expected = maritalStatus;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Create_MaritalStatus()
    {
        validator.CanCreate(maritalStatus).Returns(true);

        controller.Create(maritalStatus);

        service.Received().Create(maritalStatus);
    }

    [Fact]
    public void Create_RedirectsToIndex()
    {
        validator.CanCreate(maritalStatus).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.Create(maritalStatus);

        Assert.Same(expected, actual);
    }

    #endregion Create(MaritalStatusView maritalStatus)

    #region Details(String id)

    [Fact]
    public void Details_ReturnsNotEmptyView()
    {
        service.Get<MaritalStatusView>(maritalStatus.Id).Returns(maritalStatus);

        object expected = NotEmptyView(controller, maritalStatus);
        object actual = controller.Details(maritalStatus.Id);

        Assert.Same(expected, actual);
    }

    #endregion Details(String id)

    #region Edit(String id)

    [Fact]
    public void Edit_ReturnsNotEmptyView()
    {
        service.Get<MaritalStatusView>(maritalStatus.Id).Returns(maritalStatus);

        object expected = NotEmptyView(controller, maritalStatus);
        object actual = controller.Edit(maritalStatus.Id);

        Assert.Same(expected, actual);
    }

    #endregion Edit(String id)

    #region Edit(MaritalStatusView maritalStatus)

    [Fact]
    public void Edit_CanNotEdit_ReturnsSameView()
    {
        validator.CanEdit(maritalStatus).Returns(false);

        object actual = (controller.Edit(maritalStatus) as ViewResult).ViewData.Model;
        object expected = maritalStatus;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Edit_MaritalStatus()
    {
        validator.CanEdit(maritalStatus).Returns(true);

        controller.Edit(maritalStatus);

        service.Received().Edit(maritalStatus);
    }

    [Fact]
    public void Edit_RedirectsToIndex()
    {
        validator.CanEdit(maritalStatus).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.Edit(maritalStatus);

        Assert.Same(expected, actual);
    }

    #endregion Edit(MaritalStatusView maritalStatus)

    #region Delete(String id)

    [Fact]
    public void Delete_ReturnsNotEmptyView()
    {
        service.Get<MaritalStatusView>(maritalStatus.Id).Returns(maritalStatus);

        object expected = NotEmptyView(controller, maritalStatus);
        object actual = controller.Delete(maritalStatus.Id);

        Assert.Same(expected, actual);
    }

    #endregion Delete(String id)

    #region DeleteConfirmed(String id)

    [Fact]
    public void DeleteConfirmed_DeletesMaritalStatus()
    {
        controller.DeleteConfirmed(maritalStatus.Id);

        service.Received().Delete(maritalStatus.Id);
    }

    [Fact]
    public void Delete_RedirectsToIndex()
    {
        object expected = RedirectToAction(controller, "Index");
        object actual = controller.DeleteConfirmed(maritalStatus.Id);

        Assert.Same(expected, actual);
    }

    #endregion DeleteConfirmed(String id)
}
