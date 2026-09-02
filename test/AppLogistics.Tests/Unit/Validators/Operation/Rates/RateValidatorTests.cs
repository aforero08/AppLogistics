using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Tests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AppLogistics.Validators.Tests;

public class RateValidatorTests
{
    private RateValidator validator;
    private DbContext context;
    private Rate rate;

    public RateValidatorTests()
    {
        context = TestFixture.Create().Drop();
        validator = new RateValidator(new UnitOfWork(context, TestFixture.Mapper));

        context.Set<Rate>().Add(rate = ObjectsFactory.CreateRate());
        context.SaveChanges();
    }

    #region CanCreate(RateView view)

    [Fact]
    public void CanCreate_InvalidState_ReturnsFalse()
    {
        validator.ModelState.AddModelError("Test", "Test");

        Assert.False(validator.CanCreate(ObjectsFactory.CreateRateCreateEditView(1)));
    }

    [Fact]
    public void CanCreate_ValidRate()
    {
        Assert.True(validator.CanCreate(ObjectsFactory.CreateRateCreateEditView(1)));
        Assert.Empty(validator.ModelState);
        Assert.Empty(validator.Alerts);
    }

    #endregion CanCreate(RateView view)

    #region CanEdit(RateView view)

    [Fact]
    public void CanEdit_InvalidState_ReturnsFalse()
    {
        validator.ModelState.AddModelError("Test", "Test");

        Assert.False(validator.CanEdit(ObjectsFactory.CreateRateCreateEditView(rate.Id)));
    }

    [Fact]
    public void CanEdit_ToSameName()
    {
        RateCreateEditView view = ObjectsFactory.CreateRateCreateEditView(rate.Id);
        view.Name = rate.Name;

        Assert.True(validator.CanEdit(view));
        Assert.Empty(validator.Alerts);
    }

    [Fact]
    public void CanEdit_UsedName_ReturnsFalse()
    {
        RateCreateEditView view = ObjectsFactory.CreateRateCreateEditView(rate.Id + 1);
        view.Name = rate.Name;

        Assert.False(validator.CanEdit(view));
        Assert.NotEmpty(validator.Alerts);
    }

    [Fact]
    public void CanEdit_ValidRate()
    {
        Assert.True(validator.CanEdit(ObjectsFactory.CreateRateCreateEditView(rate.Id)));
        Assert.Empty(validator.ModelState);
        Assert.Empty(validator.Alerts);
    }

    #endregion CanEdit(RateView view)
}
