using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Tests;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace AppLogistics.Validators.Tests
{
    public class EthnicGroupValidatorTests
    {
        private EthnicGroupValidator validator;
        private DbContext context;
        private EthnicGroup ethnicGroup;

        public EthnicGroupValidatorTests()
        {
            context = TestingContext.Create();
            validator = new EthnicGroupValidator(new UnitOfWork(TestingContext.Create(), TestingContext.Mapper));

            context.Set<EthnicGroup>().Add(ethnicGroup = ObjectsFactory.CreateEthnicGroup());
            context.SaveChanges();
        }

        #region CanCreate(EthnicGroupView view)

        [Fact]
        public void CanCreate_InvalidState_ReturnsFalse()
        {
            validator.ModelState.AddModelError("Test", "Test");

            Assert.False(validator.CanCreate(ObjectsFactory.CreateEthnicGroupView(1)));
        }

        [Fact]
        public void CanCreate_ValidEthnicGroup()
        {
            Assert.True(validator.CanCreate(ObjectsFactory.CreateEthnicGroupView(1)));
            Assert.Empty(validator.ModelState);
            Assert.Empty(validator.Alerts);
        }

        #endregion

        #region CanEdit(EthnicGroupView view)

        [Fact]
        public void CanEdit_InvalidState_ReturnsFalse()
        {
            validator.ModelState.AddModelError("Test", "Test");

            Assert.False(validator.CanEdit(ObjectsFactory.CreateEthnicGroupView(ethnicGroup.Id)));
        }

        [Fact]
        public void CanEdit_ValidEthnicGroup()
        {
            Assert.True(validator.CanEdit(ObjectsFactory.CreateEthnicGroupView(ethnicGroup.Id)));
            Assert.Empty(validator.ModelState);
            Assert.Empty(validator.Alerts);
        }

        #endregion
    }
}
