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

public class EthnicGroupsControllerTests : ControllerTests
{
    private EthnicGroupsController controller;
    private IEthnicGroupValidator validator;
    private IEthnicGroupService service;
    private EthnicGroupView ethnicGroup;

    public EthnicGroupsControllerTests()
    {
        validator = Substitute.For<IEthnicGroupValidator>();
        validator.CanDelete(Arg.Any<int>()).Returns(true);
        service = Substitute.For<IEthnicGroupService>();

        ethnicGroup = ObjectsFactory.CreateEthnicGroupView();

        controller = Substitute.ForPartsOf<EthnicGroupsController>(validator, service);
        controller.ControllerContext.RouteData = new RouteData();
    }

    #region Index()

    [Fact]
    public void Index_ReturnsEthnicGroupViews()
    {
        service.GetViews().Returns(new EthnicGroupView[0].AsQueryable());

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

    #region Create(EthnicGroupView ethnicGroup)

    [Fact]
    public void Create_ProtectsFromOverpostingId()
    {
        ProtectsFromOverpostingId(controller, "Create");
    }

    [Fact]
    public void Create_CanNotCreate_ReturnsSameView()
    {
        validator.CanCreate(ethnicGroup).Returns(false);

        object actual = (controller.Create(ethnicGroup) as ViewResult).ViewData.Model;
        object expected = ethnicGroup;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Create_EthnicGroup()
    {
        validator.CanCreate(ethnicGroup).Returns(true);

        controller.Create(ethnicGroup);

        service.Received().Create(ethnicGroup);
    }

    [Fact]
    public void Create_RedirectsToIndex()
    {
        validator.CanCreate(ethnicGroup).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.Create(ethnicGroup);

        Assert.Same(expected, actual);
    }

    #endregion

    #region Details(String id)

    [Fact]
    public void Details_ReturnsNotEmptyView()
    {
        service.Get<EthnicGroupView>(ethnicGroup.Id).Returns(ethnicGroup);

        object expected = NotEmptyView(controller, ethnicGroup);
        object actual = controller.Details(ethnicGroup.Id);

        Assert.Same(expected, actual);
    }

    #endregion

    #region Edit(String id)

    [Fact]
    public void Edit_ReturnsNotEmptyView()
    {
        service.Get<EthnicGroupView>(ethnicGroup.Id).Returns(ethnicGroup);

        object expected = NotEmptyView(controller, ethnicGroup);
        object actual = controller.Edit(ethnicGroup.Id);

        Assert.Same(expected, actual);
    }

    #endregion

    #region Edit(EthnicGroupView ethnicGroup)

    [Fact]
    public void Edit_CanNotEdit_ReturnsSameView()
    {
        validator.CanEdit(ethnicGroup).Returns(false);

        object actual = (controller.Edit(ethnicGroup) as ViewResult).ViewData.Model;
        object expected = ethnicGroup;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Edit_EthnicGroup()
    {
        validator.CanEdit(ethnicGroup).Returns(true);

        controller.Edit(ethnicGroup);

        service.Received().Edit(ethnicGroup);
    }

    [Fact]
    public void Edit_RedirectsToIndex()
    {
        validator.CanEdit(ethnicGroup).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.Edit(ethnicGroup);

        Assert.Same(expected, actual);
    }

    #endregion

    #region Delete(String id)

    [Fact]
    public void Delete_ReturnsNotEmptyView()
    {
        service.Get<EthnicGroupView>(ethnicGroup.Id).Returns(ethnicGroup);

        object expected = NotEmptyView(controller, ethnicGroup);
        object actual = controller.Delete(ethnicGroup.Id);

        Assert.Same(expected, actual);
    }

    #endregion

    #region DeleteConfirmed(String id)

    [Fact]
    public void DeleteConfirmed_DeletesEthnicGroup()
    {
        controller.DeleteConfirmed(ethnicGroup.Id);

        service.Received().Delete(ethnicGroup.Id);
    }

    [Fact]
    public void Delete_RedirectsToIndex()
    {
        object expected = RedirectToAction(controller, "Index");
        object actual = controller.DeleteConfirmed(ethnicGroup.Id);

        Assert.Same(expected, actual);
    }

    #endregion
}
