using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Tests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AppLogistics.Validators.Tests;

public class CountryValidatorTests
{
    private CountryValidator validator;
    private DbContext context;
    private Country country;

    public CountryValidatorTests()
    {
        context = TestFixture.Create().Drop();
        validator = new CountryValidator(new UnitOfWork(context, TestFixture.Mapper));

        context.Set<Country>().Add(country = ObjectsFactory.CreateCountry());
        context.SaveChanges();
    }

    #region CanCreate(CountryView view)

    [Fact]
    public void CanCreate_InvalidState_ReturnsFalse()
    {
        validator.ModelState.AddModelError("Test", "Test");

        Assert.False(validator.CanCreate(ObjectsFactory.CreateCountryView(1)));
    }

    [Fact]
    public void CanCreate_ValidCountry()
    {
        Assert.True(validator.CanCreate(ObjectsFactory.CreateCountryView(1)));
        Assert.Empty(validator.ModelState);
        Assert.Empty(validator.Alerts);
    }

    #endregion

    #region CanEdit(CountryView view)

    [Fact]
    public void CanEdit_InvalidState_ReturnsFalse()
    {
        validator.ModelState.AddModelError("Test", "Test");

        Assert.False(validator.CanEdit(ObjectsFactory.CreateCountryView(country.Id)));
    }

    [Fact]
    public void CanEdit_ValidCountry()
    {
        CountryView view = ObjectsFactory.CreateCountryView(country.Id);
        view.Name = country.Name;

        Assert.True(validator.CanEdit(view));
        Assert.Empty(validator.ModelState);
        Assert.Empty(validator.Alerts);
    }

    [Fact]
    public void CanEdit_DuplicateName_ReturnsFalse()
    {
        Country otherCountry = ObjectsFactory.CreateCountry();
        otherCountry.Name = "OtherName";
        context.Set<Country>().Add(otherCountry);
        context.SaveChanges();

        CountryView view = ObjectsFactory.CreateCountryView(country.Id);
        view.Name = otherCountry.Name.ToLowerInvariant();

        Assert.False(validator.CanEdit(view));
        Assert.NotEmpty(validator.Alerts);
    }

    #endregion
}
