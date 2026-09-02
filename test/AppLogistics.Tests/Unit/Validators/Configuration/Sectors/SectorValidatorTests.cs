using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Tests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AppLogistics.Validators.Tests;

public class SectorValidatorTests
{
    private SectorValidator validator;
    private DbContext context;
    private Sector sector;

    public SectorValidatorTests()
    {
        context = TestFixture.Create().Drop();
        validator = new SectorValidator(new UnitOfWork(context, TestFixture.Mapper));

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
        SectorView view = ObjectsFactory.CreateSectorView(sector.Id);
        view.Name = sector.Name;

        Assert.True(validator.CanEdit(view));
        Assert.Empty(validator.ModelState);
        Assert.Empty(validator.Alerts);
    }

    [Fact]
    public void CanEdit_DuplicateName_ReturnsFalse()
    {
        Sector otherSector = ObjectsFactory.CreateSector();
        otherSector.Name = "OtherName";
        context.Set<Sector>().Add(otherSector);
        context.SaveChanges();

        SectorView view = ObjectsFactory.CreateSectorView(sector.Id);
        view.Name = otherSector.Name.ToLowerInvariant();

        Assert.False(validator.CanEdit(view));
        Assert.NotEmpty(validator.Alerts);
    }

    #endregion
}
