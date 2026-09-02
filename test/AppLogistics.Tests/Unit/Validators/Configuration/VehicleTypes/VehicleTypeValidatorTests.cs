using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Tests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AppLogistics.Validators.Tests;

public class VehicleTypeValidatorTests
{
    private VehicleTypeValidator validator;
    private DbContext context;
    private VehicleType vehicleType;

    public VehicleTypeValidatorTests()
    {
        context = TestFixture.Create().Drop();
        validator = new VehicleTypeValidator(new UnitOfWork(context, TestFixture.Mapper));

        context.Set<VehicleType>().Add(vehicleType = ObjectsFactory.CreateVehicleType());
        context.SaveChanges();
    }

    #region CanCreate(VehicleTypeView view)

    [Fact]
    public void CanCreate_InvalidState_ReturnsFalse()
    {
        validator.ModelState.AddModelError("Test", "Test");

        Assert.False(validator.CanCreate(ObjectsFactory.CreateVehicleTypeView(1)));
    }

    [Fact]
    public void CanCreate_ValidVehicleType()
    {
        Assert.True(validator.CanCreate(ObjectsFactory.CreateVehicleTypeView(1)));
        Assert.Empty(validator.ModelState);
        Assert.Empty(validator.Alerts);
    }

    #endregion CanCreate(VehicleTypeView view)

    #region CanEdit(VehicleTypeView view)

    [Fact]
    public void CanEdit_InvalidState_ReturnsFalse()
    {
        validator.ModelState.AddModelError("Test", "Test");

        Assert.False(validator.CanEdit(ObjectsFactory.CreateVehicleTypeView(vehicleType.Id)));
    }

    [Fact]
    public void CanEdit_ValidVehicleType()
    {
        VehicleTypeView view = ObjectsFactory.CreateVehicleTypeView(vehicleType.Id);
        view.Name = vehicleType.Name;

        Assert.True(validator.CanEdit(view));
        Assert.Empty(validator.ModelState);
        Assert.Empty(validator.Alerts);
    }

    [Fact]
    public void CanEdit_DuplicateName_ReturnsFalse()
    {
        VehicleType otherVehicleType = ObjectsFactory.CreateVehicleType();
        otherVehicleType.Name = "OtherName";
        context.Set<VehicleType>().Add(otherVehicleType);
        context.SaveChanges();

        VehicleTypeView view = ObjectsFactory.CreateVehicleTypeView(vehicleType.Id);
        view.Name = otherVehicleType.Name.ToLowerInvariant();

        Assert.False(validator.CanEdit(view));
        Assert.NotEmpty(validator.Alerts);
    }

    #endregion CanEdit(VehicleTypeView view)
}
