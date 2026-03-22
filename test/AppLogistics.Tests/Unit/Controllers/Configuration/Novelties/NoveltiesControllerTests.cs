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

public class NoveltiesControllerTests : ControllerTests
{
    private NoveltiesController controller;
    private INoveltyValidator validator;
    private INoveltyService service;
    private NoveltyView novelty;

    public NoveltiesControllerTests()
    {
        validator = Substitute.For<INoveltyValidator>();
        validator.CanDelete(Arg.Any<int>()).Returns(true);
        service = Substitute.For<INoveltyService>();

        novelty = ObjectsFactory.CreateNoveltyView();

        controller = Substitute.ForPartsOf<NoveltiesController>(validator, service);
        controller.ControllerContext.RouteData = new RouteData();
    }

    #region Index()

    [Fact]
    public void Index_ReturnsNoveltyViews()
    {
        service.GetViews().Returns(new NoveltyView[0].AsQueryable());

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

    #region Create(NoveltyView novelty)

    [Fact]
    public void Create_ProtectsFromOverpostingId()
    {
        ProtectsFromOverpostingId(controller, "Create");
    }

    [Fact]
    public void Create_CanNotCreate_ReturnsSameView()
    {
        validator.CanCreate(novelty).Returns(false);

        object actual = (controller.Create(novelty) as ViewResult).ViewData.Model;
        object expected = novelty;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Create_Novelty()
    {
        validator.CanCreate(novelty).Returns(true);

        controller.Create(novelty);

        service.Received().Create(novelty);
    }

    [Fact]
    public void Create_RedirectsToIndex()
    {
        validator.CanCreate(novelty).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.Create(novelty);

        Assert.Same(expected, actual);
    }

    #endregion

    #region Details(String id)

    [Fact]
    public void Details_ReturnsNotEmptyView()
    {
        service.Get<NoveltyView>(novelty.Id).Returns(novelty);

        object expected = NotEmptyView(controller, novelty);
        object actual = controller.Details(novelty.Id);

        Assert.Same(expected, actual);
    }

    #endregion

    #region Edit(String id)

    [Fact]
    public void Edit_ReturnsNotEmptyView()
    {
        service.Get<NoveltyView>(novelty.Id).Returns(novelty);

        object expected = NotEmptyView(controller, novelty);
        object actual = controller.Edit(novelty.Id);

        Assert.Same(expected, actual);
    }

    #endregion

    #region Edit(NoveltyView novelty)

    [Fact]
    public void Edit_CanNotEdit_ReturnsSameView()
    {
        validator.CanEdit(novelty).Returns(false);

        object actual = (controller.Edit(novelty) as ViewResult).ViewData.Model;
        object expected = novelty;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Edit_Novelty()
    {
        validator.CanEdit(novelty).Returns(true);

        controller.Edit(novelty);

        service.Received().Edit(novelty);
    }

    [Fact]
    public void Edit_RedirectsToIndex()
    {
        validator.CanEdit(novelty).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.Edit(novelty);

        Assert.Same(expected, actual);
    }

    #endregion

    #region Delete(String id)

    [Fact]
    public void Delete_ReturnsNotEmptyView()
    {
        service.Get<NoveltyView>(novelty.Id).Returns(novelty);

        object expected = NotEmptyView(controller, novelty);
        object actual = controller.Delete(novelty.Id);

        Assert.Same(expected, actual);
    }

    #endregion

    #region DeleteConfirmed(String id)

    [Fact]
    public void DeleteConfirmed_DeletesNovelty()
    {
        controller.DeleteConfirmed(novelty.Id);

        service.Received().Delete(novelty.Id);
    }

    [Fact]
    public void Delete_RedirectsToIndex()
    {
        object expected = RedirectToAction(controller, "Index");
        object actual = controller.DeleteConfirmed(novelty.Id);

        Assert.Same(expected, actual);
    }

    #endregion
}
