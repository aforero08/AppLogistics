using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Tests;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace AppLogistics.Validators.Tests
{
    public class EpsValidatorTests
    {
        private EpsValidator validator;
        private DbContext context;
        private Eps eps;

        public EpsValidatorTests()
        {
            context = TestingContext.Create();
            validator = new EpsValidator(new UnitOfWork(TestingContext.Create(), TestingContext.Mapper));

            context.Set<Eps>().Add(eps = ObjectsFactory.CreateEps());
            context.SaveChanges();
        }

        #region CanCreate(EpsView view)

        [Fact]
        public void CanCreate_InvalidState_ReturnsFalse()
        {
            validator.ModelState.AddModelError("Test", "Test");

            Assert.False(validator.CanCreate(ObjectsFactory.CreateEpsView(1)));
        }

        [Fact]
        public void CanCreate_ValidEps()
        {
            Assert.True(validator.CanCreate(ObjectsFactory.CreateEpsView(1)));
            Assert.Empty(validator.ModelState);
            Assert.Empty(validator.Alerts);
        }

        #endregion CanCreate(EpsView view)

        #region CanEdit(EpsView view)

        [Fact]
        public void CanEdit_InvalidState_ReturnsFalse()
        {
            validator.ModelState.AddModelError("Test", "Test");

            Assert.False(validator.CanEdit(ObjectsFactory.CreateEpsView(eps.Id)));
        }

        [Fact]
        public void CanEdit_ValidEps()
        {
            Assert.True(validator.CanEdit(ObjectsFactory.CreateEpsView(eps.Id)));
            Assert.Empty(validator.ModelState);
            Assert.Empty(validator.Alerts);
        }

        #endregion CanEdit(EpsView view)
    }
}
