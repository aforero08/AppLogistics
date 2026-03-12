using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Tests;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace AppLogistics.Validators.Tests
{
    public class CarrierValidatorTests
    {
        private CarrierValidator validator;
        private DbContext context;
        private Carrier carrier;

        public CarrierValidatorTests()
        {
            context = TestingContext.Create();
            validator = new CarrierValidator(new UnitOfWork(TestingContext.Create(), TestingContext.Mapper));

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
            Assert.True(validator.CanEdit(ObjectsFactory.CreateCarrierView(carrier.Id)));
            Assert.Empty(validator.ModelState);
            Assert.Empty(validator.Alerts);
        }

        #endregion CanEdit(CarrierView view)
    }
}
