using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Tests;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace AppLogistics.Validators.Tests;

public class SectorValidatorTests
{
    private SectorValidator validator;
    private DbContext context;
    private Sector sector;

    public SectorValidatorTests()
    {
        context = TestingContext.Create();
        validator = new SectorValidator(new UnitOfWork(TestingContext.Create(), TestingContext.Mapper));

        context.Set<Sector>().Add(sector = ObjectsFactory.CreateSector());
        context.SaveChanges();
    }

    #region CanCreate(SectorView view)

    [Fact]
    public void CanCreate_InvalidState_ReturnsFalse()
    {
        validator.ModelState.AddModelError("Test", "Test");

        Assert.False(validator.CanCreate(ObjectsFactory.CreateSectorView(1)));
    }

    [Fact]
    public void CanCreate_ValidSector()
    {
        Assert.True(validator.CanCreate(ObjectsFactory.CreateSectorView(1)));
        Assert.Empty(validator.ModelState);
        Assert.Empty(validator.Alerts);
    }

    #endregion

    #region CanEdit(SectorView view)

    [Fact]
    public void CanEdit_InvalidState_ReturnsFalse()
    {
        validator.ModelState.AddModelError("Test", "Test");

        Assert.False(validator.CanEdit(ObjectsFactory.CreateSectorView(sector.Id)));
    }

    [Fact]
    public void CanEdit_ValidSector()
    {
        Assert.True(validator.CanEdit(ObjectsFactory.CreateSectorView(sector.Id)));
        Assert.Empty(validator.ModelState);
        Assert.Empty(validator.Alerts);
    }

    #endregion
}
