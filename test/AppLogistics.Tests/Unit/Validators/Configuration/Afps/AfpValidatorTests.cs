using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Tests;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace AppLogistics.Validators.Tests;

public class AfpValidatorTests
{
    private AfpValidator validator;
    private DbContext context;
    private Afp afp;

    public AfpValidatorTests()
    {
        context = TestingContext.Create();
        validator = new AfpValidator(new UnitOfWork(TestingContext.Create(), TestingContext.Mapper));

        context.Set<Afp>().Add(afp = ObjectsFactory.CreateAfp());
        context.SaveChanges();
    }

    #region CanCreate(AfpView view)

    [Fact]
    public void CanCreate_InvalidState_ReturnsFalse()
    {
        validator.ModelState.AddModelError("Test", "Test");

        Assert.False(validator.CanCreate(ObjectsFactory.CreateAfpView(1)));
    }

    [Fact]
    public void CanCreate_ValidAfp()
    {
        Assert.True(validator.CanCreate(ObjectsFactory.CreateAfpView(1)));
        Assert.Empty(validator.ModelState);
        Assert.Empty(validator.Alerts);
    }

    #endregion CanCreate(AfpView view)

    #region CanEdit(AfpView view)

    [Fact]
    public void CanEdit_InvalidState_ReturnsFalse()
    {
        validator.ModelState.AddModelError("Test", "Test");

        Assert.False(validator.CanEdit(ObjectsFactory.CreateAfpView(afp.Id)));
    }

    [Fact]
    public void CanEdit_ValidAfp()
    {
        Assert.True(validator.CanEdit(ObjectsFactory.CreateAfpView(afp.Id)));
        Assert.Empty(validator.ModelState);
        Assert.Empty(validator.Alerts);
    }

    #endregion CanEdit(AfpView view)
}
