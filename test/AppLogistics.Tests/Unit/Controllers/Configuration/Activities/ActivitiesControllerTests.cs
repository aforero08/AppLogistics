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

public class ActivitiesControllerTests : ControllerTests
{
    private ActivitiesController controller;
    private IActivityValidator validator;
    private IActivityService service;
    private ActivityView activity;

    public ActivitiesControllerTests()
    {
        validator = Substitute.For<IActivityValidator>();
        validator.CanDelete(Arg.Any<int>()).Returns(true);
        service = Substitute.For<IActivityService>();

        activity = ObjectsFactory.CreateActivityView();

        controller = Substitute.ForPartsOf<ActivitiesController>(validator, service);
        controller.ControllerContext.RouteData = new RouteData();
    }

    #region Index()

    [Fact]
    public void Index_ReturnsActivityViews()
    {
        service.GetViews().Returns(new ActivityView[0].AsQueryable());

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

    #region Create(ActivityView activity)

    [Fact]
    public void Create_ProtectsFromOverpostingId()
    {
        ProtectsFromOverpostingId(controller, "Create");
    }

    [Fact]
    public void Create_CanNotCreate_ReturnsSameView()
    {
        validator.CanCreate(activity).Returns(false);

        object actual = (controller.Create(activity) as ViewResult).ViewData.Model;
        object expected = activity;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Create_Activity()
    {
        validator.CanCreate(activity).Returns(true);

        controller.Create(activity);

        service.Received().Create(activity);
    }

    [Fact]
    public void Create_RedirectsToIndex()
    {
        validator.CanCreate(activity).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.Create(activity);

        Assert.Same(expected, actual);
    }

    #endregion Create(ActivityView activity)

    #region Details(String id)

    [Fact]
    public void Details_ReturnsNotEmptyView()
    {
        service.Get<ActivityView>(activity.Id).Returns(activity);

        object expected = NotEmptyView(controller, activity);
        object actual = controller.Details(activity.Id);

        Assert.Same(expected, actual);
    }

    #endregion Details(String id)

    #region Edit(String id)

    [Fact]
    public void Edit_ReturnsNotEmptyView()
    {
        service.Get<ActivityView>(activity.Id).Returns(activity);

        object expected = NotEmptyView(controller, activity);
        object actual = controller.Edit(activity.Id);

        Assert.Same(expected, actual);
    }

    #endregion Edit(String id)

    #region Edit(ActivityView activity)

    [Fact]
    public void Edit_CanNotEdit_ReturnsSameView()
    {
        validator.CanEdit(activity).Returns(false);

        object actual = (controller.Edit(activity) as ViewResult).ViewData.Model;
        object expected = activity;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Edit_Activity()
    {
        validator.CanEdit(activity).Returns(true);

        controller.Edit(activity);

        service.Received().Edit(activity);
    }

    [Fact]
    public void Edit_RedirectsToIndex()
    {
        validator.CanEdit(activity).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.Edit(activity);

        Assert.Same(expected, actual);
    }

    #endregion Edit(ActivityView activity)

    #region Delete(String id)

    [Fact]
    public void Delete_ReturnsNotEmptyView()
    {
        service.Get<ActivityView>(activity.Id).Returns(activity);

        object expected = NotEmptyView(controller, activity);
        object actual = controller.Delete(activity.Id);

        Assert.Same(expected, actual);
    }

    #endregion Delete(String id)

    #region DeleteConfirmed(String id)

    [Fact]
    public void DeleteConfirmed_DeletesActivity()
    {
        controller.DeleteConfirmed(activity.Id);

        service.Received().Delete(activity.Id);
    }

    [Fact]
    public void Delete_RedirectsToIndex()
    {
        object expected = RedirectToAction(controller, "Index");
        object actual = controller.DeleteConfirmed(activity.Id);

        Assert.Same(expected, actual);
    }

    #endregion DeleteConfirmed(String id)
}
