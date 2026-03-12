using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Tests;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace AppLogistics.Validators.Tests
{
    public class MaritalStatusValidatorTests
    {
        private MaritalStatusValidator validator;
        private DbContext context;
        private MaritalStatus maritalStatus;

        public MaritalStatusValidatorTests()
        {
            context = TestingContext.Create();
            validator = new MaritalStatusValidator(new UnitOfWork(TestingContext.Create(), TestingContext.Mapper));

            context.Set<MaritalStatus>().Add(maritalStatus = ObjectsFactory.CreateMaritalStatus());
            context.SaveChanges();
        }

        #region CanCreate(MaritalStatusView view)

        [Fact]
        public void CanCreate_InvalidState_ReturnsFalse()
        {
            validator.ModelState.AddModelError("Test", "Test");

            Assert.False(validator.CanCreate(ObjectsFactory.CreateMaritalStatusView(1)));
        }

        [Fact]
        public void CanCreate_ValidMaritalStatus()
        {
            Assert.True(validator.CanCreate(ObjectsFactory.CreateMaritalStatusView(1)));
            Assert.Empty(validator.ModelState);
            Assert.Empty(validator.Alerts);
        }

        #endregion CanCreate(MaritalStatusView view)

        #region CanEdit(MaritalStatusView view)

        [Fact]
        public void CanEdit_InvalidState_ReturnsFalse()
        {
            validator.ModelState.AddModelError("Test", "Test");

            Assert.False(validator.CanEdit(ObjectsFactory.CreateMaritalStatusView(maritalStatus.Id)));
        }

        [Fact]
        public void CanEdit_ValidMaritalStatus()
        {
            Assert.True(validator.CanEdit(ObjectsFactory.CreateMaritalStatusView(maritalStatus.Id)));
            Assert.Empty(validator.ModelState);
            Assert.Empty(validator.Alerts);
        }

        #endregion CanEdit(MaritalStatusView view)
    }
}
