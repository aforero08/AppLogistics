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

public class CountriesControllerTests : ControllerTests
{
    private CountriesController controller;
    private ICountryValidator validator;
    private ICountryService service;
    private CountryView country;

    public CountriesControllerTests()
    {
        validator = Substitute.For<ICountryValidator>();
        validator.CanDelete(Arg.Any<int>()).Returns(true);
        service = Substitute.For<ICountryService>();

        country = ObjectsFactory.CreateCountryView();

        controller = Substitute.ForPartsOf<CountriesController>(validator, service);
        controller.ControllerContext.RouteData = new RouteData();
    }

    #region Index()

    [Fact]
    public void Index_ReturnsCountryViews()
    {
        service.GetViews().Returns(new CountryView[0].AsQueryable());

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

    #region Create(CountryView country)

    [Fact]
    public void Create_ProtectsFromOverpostingId()
    {
        ProtectsFromOverpostingId(controller, "Create");
    }

    [Fact]
    public void Create_CanNotCreate_ReturnsSameView()
    {
        validator.CanCreate(country).Returns(false);

        object actual = (controller.Create(country) as ViewResult).ViewData.Model;
        object expected = country;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Create_Country()
    {
        validator.CanCreate(country).Returns(true);

        controller.Create(country);

        service.Received().Create(country);
    }

    [Fact]
    public void Create_RedirectsToIndex()
    {
        validator.CanCreate(country).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.Create(country);

        Assert.Same(expected, actual);
    }

    #endregion

    #region Details(String id)

    [Fact]
    public void Details_ReturnsNotEmptyView()
    {
        service.Get<CountryView>(country.Id).Returns(country);

        object expected = NotEmptyView(controller, country);
        object actual = controller.Details(country.Id);

        Assert.Same(expected, actual);
    }

    #endregion

    #region Edit(String id)

    [Fact]
    public void Edit_ReturnsNotEmptyView()
    {
        service.Get<CountryView>(country.Id).Returns(country);

        object expected = NotEmptyView(controller, country);
        object actual = controller.Edit(country.Id);

        Assert.Same(expected, actual);
    }

    #endregion

    #region Edit(CountryView country)

    [Fact]
    public void Edit_CanNotEdit_ReturnsSameView()
    {
        validator.CanEdit(country).Returns(false);

        object actual = (controller.Edit(country) as ViewResult).ViewData.Model;
        object expected = country;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Edit_Country()
    {
        validator.CanEdit(country).Returns(true);

        controller.Edit(country);

        service.Received().Edit(country);
    }

    [Fact]
    public void Edit_RedirectsToIndex()
    {
        validator.CanEdit(country).Returns(true);

        object expected = RedirectToAction(controller, "Index");
        object actual = controller.Edit(country);

        Assert.Same(expected, actual);
    }

    #endregion

    #region Delete(String id)

    [Fact]
    public void Delete_ReturnsNotEmptyView()
    {
        service.Get<CountryView>(country.Id).Returns(country);

        object expected = NotEmptyView(controller, country);
        object actual = controller.Delete(country.Id);

        Assert.Same(expected, actual);
    }

    #endregion

    #region DeleteConfirmed(String id)

    [Fact]
    public void DeleteConfirmed_DeletesCountry()
    {
        controller.DeleteConfirmed(country.Id);

        service.Received().Delete(country.Id);
    }

    [Fact]
    public void Delete_RedirectsToIndex()
    {
        object expected = RedirectToAction(controller, "Index");
        object actual = controller.DeleteConfirmed(country.Id);

        Assert.Same(expected, actual);
    }

    #endregion
}
