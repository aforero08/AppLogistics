using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Resources;
using AppLogistics.Tests;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace AppLogistics.Validators.Tests
{
    public class RoleValidatorTests
    {
        private RoleValidator validator;
        private DbContext context;
        private Role role;

        public RoleValidatorTests()
        {
            context = TestingContext.Create();
            validator = new RoleValidator(new UnitOfWork(TestingContext.Create(), TestingContext.Mapper));

            context.Add(role = ObjectsFactory.CreateRole());
            context.SaveChanges();
        }

        #region CanCreate(RoleView view)

        [Fact]
        public void CanCreate_InvalidState_ReturnsFalse()
        {
            validator.ModelState.AddModelError("Test", "Test");

            Assert.False(validator.CanCreate(ObjectsFactory.CreateRoleView(1)));
        }

        [Fact]
        public void CanCreate_UsedTitle_ReturnsFalse()
        {
            RoleView view = ObjectsFactory.CreateRoleView(1);
            view.Title = role.Title.ToLower();

            bool canCreate = validator.CanCreate(view);

            Assert.False(canCreate);
            Assert.Single(validator.ModelState);
            Assert.Equal(Validation.For<RoleView>("UniqueTitle"), validator.ModelState["Title"].Errors.Single().ErrorMessage);
        }

        [Fact]
        public void CanCreate_ValidRole()
        {
            Assert.True(validator.CanCreate(ObjectsFactory.CreateRoleView(1)));
            Assert.Empty(validator.ModelState);
            Assert.Empty(validator.Alerts);
        }

        #endregion CanCreate(RoleView view)

        #region CanEdit(RoleView view)

        [Fact]
        public void CanEdit_InvalidState_ReturnsFalse()
        {
            validator.ModelState.AddModelError("Test", "Test");

            Assert.False(validator.CanEdit(ObjectsFactory.CreateRoleView(role.Id)));
        }

        [Fact]
        public void CanEdit_UsedTitle_ReturnsFalse()
        {
            RoleView view = ObjectsFactory.CreateRoleView(1);
            view.Title = role.Title.ToLower();

            bool canEdit = validator.CanEdit(view);

            Assert.False(canEdit);
            Assert.Single(validator.ModelState);
            Assert.Equal(Validation.For<RoleView>("UniqueTitle"), validator.ModelState["Title"].Errors.Single().ErrorMessage);
        }

        [Fact]
        public void CanEdit_ValidRole()
        {
            Assert.True(validator.CanEdit(ObjectsFactory.CreateRoleView(role.Id)));
            Assert.Empty(validator.ModelState);
            Assert.Empty(validator.Alerts);
        }

        #endregion CanEdit(RoleView view)
    }
}
