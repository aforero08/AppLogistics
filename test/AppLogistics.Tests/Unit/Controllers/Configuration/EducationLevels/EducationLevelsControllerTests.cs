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

public class EducationLevelsControllerTests : ControllerTests
{
    private EducationLevelsController controller;
    private IEducationLevelValidator validator;
    private IEducationLevelService service;
    private EducationLevelView educationLevel;

    public EducationLevelsControllerTests()
    {
        validator = Substitute.For<IEducationLevelValidator>();
        validator.CanDelete(Arg.Any<int>()).Returns(true);
        service = Substitute.For<IEducationLevelService>();

        educationLevel = ObjectsFactory.CreateEducationLevelView();

        controller = Substitute.ForPartsOf<EducationLevelsController>(validator, service);
        controller.ControllerContext.RouteData = new RouteData();
    }

    #region Index()

    [Fact]
    public void Index_ReturnsEducationLevelViews()
    {
        service.GetViews().Returns(new EducationLevelView[0].AsQueryable());

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

    #region Create(EducationLevelView educationLevel)

    [Fact]
    public void Create_ProtectsFromOverpostingId()
    {
        ProtectsFromOverpostingId(controller, "Create");
    }

    [Fact]
    public void Create_CanNotCreate_ReturnsSameView()
    {
        validator.CanCreate(educationLevel).Returns(false);

        object actual = (controller.Create(educationLevel) as ViewResult).ViewData.Model;
        object expected = educationLevel;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Create_EducationLevel()
    {
        validator.CanCreate(educationLevel).Returns(true);

        controller.Create(educationLevel);

        service.Received().Create(educationLevel);
    }

    [Fact]
    public void Create_RedirectsToIndex()
    {
        validator.CanCreate(educationLevel).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.Create(educationLevel);

        Assert.Same(expected, actual);
    }

    #endregion

    #region Details(String id)

    [Fact]
    public void Details_ReturnsNotEmptyView()
    {
        service.Get<EducationLevelView>(educationLevel.Id).Returns(educationLevel);

        object expected = NotEmptyView(controller, educationLevel);
        object actual = controller.Details(educationLevel.Id);

        Assert.Same(expected, actual);
    }

    #endregion

    #region Edit(String id)

    [Fact]
    public void Edit_ReturnsNotEmptyView()
    {
        service.Get<EducationLevelView>(educationLevel.Id).Returns(educationLevel);

        object expected = NotEmptyView(controller, educationLevel);
        object actual = controller.Edit(educationLevel.Id);

        Assert.Same(expected, actual);
    }

    #endregion

    #region Edit(EducationLevelView educationLevel)

    [Fact]
    public void Edit_CanNotEdit_ReturnsSameView()
    {
        validator.CanEdit(educationLevel).Returns(false);

        object actual = (controller.Edit(educationLevel) as ViewResult).ViewData.Model;
        object expected = educationLevel;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Edit_EducationLevel()
    {
        validator.CanEdit(educationLevel).Returns(true);

        controller.Edit(educationLevel);

        service.Received().Edit(educationLevel);
    }

    [Fact]
    public void Edit_RedirectsToIndex()
    {
        validator.CanEdit(educationLevel).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.Edit(educationLevel);

        Assert.Same(expected, actual);
    }

    #endregion

    #region Delete(String id)

    [Fact]
    public void Delete_ReturnsNotEmptyView()
    {
        service.Get<EducationLevelView>(educationLevel.Id).Returns(educationLevel);

        object expected = NotEmptyView(controller, educationLevel);
        object actual = controller.Delete(educationLevel.Id);

        Assert.Same(expected, actual);
    }

    #endregion

    #region DeleteConfirmed(String id)

    [Fact]
    public void DeleteConfirmed_DeletesEducationLevel()
    {
        controller.DeleteConfirmed(educationLevel.Id);

        service.Received().Delete(educationLevel.Id);
    }

    [Fact]
    public void Delete_RedirectsToIndex()
    {
        object expected = RedirectToAction(controller, "Index");
        object actual = controller.DeleteConfirmed(educationLevel.Id);

        Assert.Same(expected, actual);
    }

    #endregion
}
