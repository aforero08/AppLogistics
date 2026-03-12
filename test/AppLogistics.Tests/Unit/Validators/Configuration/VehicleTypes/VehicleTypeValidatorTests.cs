using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Tests;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace AppLogistics.Validators.Tests
{
    public class VehicleTypeValidatorTests
    {
        private VehicleTypeValidator validator;
        private DbContext context;
        private VehicleType vehicleType;

        public VehicleTypeValidatorTests()
        {
            context = TestingContext.Create();
            validator = new VehicleTypeValidator(new UnitOfWork(TestingContext.Create(), TestingContext.Mapper));

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
            Assert.True(validator.CanEdit(ObjectsFactory.CreateVehicleTypeView(vehicleType.Id)));
            Assert.Empty(validator.ModelState);
            Assert.Empty(validator.Alerts);
        }

        #endregion CanEdit(VehicleTypeView view)
    }
}
