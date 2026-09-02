using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Tests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AppLogistics.Validators.Tests;

public class CarrierValidatorTests
{
    private CarrierValidator validator;
    private DbContext context;
    private Carrier carrier;

    public CarrierValidatorTests()
    {
        context = TestFixture.Create().Drop();
        validator = new CarrierValidator(new UnitOfWork(context, TestFixture.Mapper));

        context.Set<Carrier>().Add(carrier = ObjectsFactory.CreateCarrier());
        context.SaveChanges();
    }

    #region CanCreate(CarrierView view)

    [Fact]
    public void CanCreate_InvalidState_ReturnsFalse()
    {
        validator.ModelState.AddModelError("Test", "Test");

        Assert.False(validator.CanCreate(ObjectsFactory.CreateCarrierView(1)));
    }

    [Fact]
    public void CanCreate_ValidCarrier()
    {
        Assert.True(validator.CanCreate(ObjectsFactory.CreateCarrierView(1)));
        Assert.Empty(validator.ModelState);
        Assert.Empty(validator.Alerts);
    }

    #endregion CanCreate(CarrierView view)

    #region CanEdit(CarrierView view)

    [Fact]
    public void CanEdit_InvalidState_ReturnsFalse()
    {
        validator.ModelState.AddModelError("Test", "Test");

        Assert.False(validator.CanEdit(ObjectsFactory.CreateCarrierView(carrier.Id)));
    }

    [Fact]
    public void CanEdit_ValidCarrier()
    {
        CarrierView view = ObjectsFactory.CreateCarrierView(carrier.Id);
        view.Name = carrier.Name;

        Assert.True(validator.CanEdit(view));
        Assert.Empty(validator.ModelState);
        Assert.Empty(validator.Alerts);
    }

    [Fact]
    public void CanEdit_DuplicateName_ReturnsFalse()
    {
        Carrier otherCarrier = ObjectsFactory.CreateCarrier();
        otherCarrier.Name = "OtherName";
        context.Set<Carrier>().Add(otherCarrier);
        context.SaveChanges();

        CarrierView view = ObjectsFactory.CreateCarrierView(carrier.Id);
        view.Name = otherCarrier.Name.ToLowerInvariant();

        Assert.False(validator.CanEdit(view));
        Assert.NotEmpty(validator.Alerts);
    }

    #endregion CanEdit(CarrierView view)
}
