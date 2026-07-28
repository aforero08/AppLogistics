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

public class BranchOfficesControllerTests : ControllerTests
{
    private BranchOfficesController controller;
    private IBranchOfficeValidator validator;
    private IBranchOfficeService service;
    private BranchOfficeView branchOffice;

    public BranchOfficesControllerTests()
    {
        validator = Substitute.For<IBranchOfficeValidator>();
        validator.CanDelete(Arg.Any<int>()).Returns(true);
        service = Substitute.For<IBranchOfficeService>();

        branchOffice = ObjectsFactory.CreateBranchOfficeView();

        controller = Substitute.ForPartsOf<BranchOfficesController>(validator, service);
        controller.ControllerContext.RouteData = new RouteData();
    }

    #region Index()

    [Fact]
    public void Index_ReturnsBranchOfficeViews()
    {
        service.GetViews().Returns(new BranchOfficeView[0].AsQueryable());

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

    #region Create(BranchOfficeView branchOffice)

    [Fact]
    public void Create_ProtectsFromOverpostingId()
    {
        ProtectsFromOverpostingId(controller, "Create");
    }

    [Fact]
    public void Create_CanNotCreate_ReturnsSameView()
    {
        validator.CanCreate(branchOffice).Returns(false);

        object actual = (controller.Create(branchOffice) as ViewResult).ViewData.Model;
        object expected = branchOffice;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Create_BranchOffice()
    {
        validator.CanCreate(branchOffice).Returns(true);

        controller.Create(branchOffice);

        service.Received().Create(branchOffice);
    }

    [Fact]
    public void Create_RedirectsToIndex()
    {
        validator.CanCreate(branchOffice).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.Create(branchOffice);

        Assert.Same(expected, actual);
    }

    #endregion Create(BranchOfficeView branchOffice)

    #region Details(String id)

    [Fact]
    public void Details_ReturnsNotEmptyView()
    {
        service.Get<BranchOfficeView>(branchOffice.Id).Returns(branchOffice);

        object expected = NotEmptyView(controller, branchOffice);
        object actual = controller.Details(branchOffice.Id);

        Assert.Same(expected, actual);
    }

    #endregion Details(String id)

    #region Edit(String id)

    [Fact]
    public void Edit_ReturnsNotEmptyView()
    {
        service.Get<BranchOfficeView>(branchOffice.Id).Returns(branchOffice);

        object expected = NotEmptyView(controller, branchOffice);
        object actual = controller.Edit(branchOffice.Id);

        Assert.Same(expected, actual);
    }

    #endregion Edit(String id)

    #region Edit(BranchOfficeView branchOffice)

    [Fact]
    public void Edit_CanNotEdit_ReturnsSameView()
    {
        validator.CanEdit(branchOffice).Returns(false);

        object actual = (controller.Edit(branchOffice) as ViewResult).ViewData.Model;
        object expected = branchOffice;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Edit_BranchOffice()
    {
        validator.CanEdit(branchOffice).Returns(true);

        controller.Edit(branchOffice);

        service.Received().Edit(branchOffice);
    }

    [Fact]
    public void Edit_RedirectsToIndex()
    {
        validator.CanEdit(branchOffice).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.Edit(branchOffice);

        Assert.Same(expected, actual);
    }

    #endregion Edit(BranchOfficeView branchOffice)

    #region Delete(String id)

    [Fact]
    public void Delete_ReturnsNotEmptyView()
    {
        service.Get<BranchOfficeView>(branchOffice.Id).Returns(branchOffice);

        object expected = NotEmptyView(controller, branchOffice);
        object actual = controller.Delete(branchOffice.Id);

        Assert.Same(expected, actual);
    }

    #endregion Delete(String id)

    #region DeleteConfirmed(String id)

    [Fact]
    public void DeleteConfirmed_DeletesBranchOffice()
    {
        controller.DeleteConfirmed(branchOffice.Id);

        service.Received().Delete(branchOffice.Id);
    }

    [Fact]
    public void Delete_RedirectsToIndex()
    {
        object expected = RedirectToAction(controller, "Index");
        object actual = controller.DeleteConfirmed(branchOffice.Id);

        Assert.Same(expected, actual);
    }

    #endregion DeleteConfirmed(String id)
}
