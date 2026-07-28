using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Tests;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace AppLogistics.Validators.Tests;

public class SexValidatorTests
{
    private SexValidator validator;
    private DbContext context;
    private Sex sex;

    public SexValidatorTests()
    {
        context = TestFixture.Create().Drop();
        validator = new SexValidator(new UnitOfWork(context, TestFixture.Mapper));

        context.Set<Sex>().Add(sex = ObjectsFactory.CreateSex());
        context.SaveChanges();
    }

    #region CanCreate(SexView view)

    [Fact]
    public void CanCreate_InvalidState_ReturnsFalse()
    {
        validator.ModelState.AddModelError("Test", "Test");

        Assert.False(validator.CanCreate(ObjectsFactory.CreateSexView(1)));
    }

    [Fact]
    public void CanCreate_ValidSex()
    {
        Assert.True(validator.CanCreate(ObjectsFactory.CreateSexView(1)));
        Assert.Empty(validator.ModelState);
        Assert.Empty(validator.Alerts);
    }

    #endregion

    #region CanEdit(SexView view)

    [Fact]
    public void CanEdit_InvalidState_ReturnsFalse()
    {
        validator.ModelState.AddModelError("Test", "Test");

        Assert.False(validator.CanEdit(ObjectsFactory.CreateSexView(sex.Id)));
    }

    [Fact]
    public void CanEdit_ValidSex()
    {
        Assert.True(validator.CanEdit(ObjectsFactory.CreateSexView(sex.Id)));
        Assert.Empty(validator.ModelState);
        Assert.Empty(validator.Alerts);
    }

    #endregion
}
