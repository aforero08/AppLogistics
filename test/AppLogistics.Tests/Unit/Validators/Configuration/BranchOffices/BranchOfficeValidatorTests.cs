using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Tests;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace AppLogistics.Validators.Tests;

public class BranchOfficeValidatorTests
{
    private BranchOfficeValidator validator;
    private DbContext context;
    private BranchOffice branchOffice;

    public BranchOfficeValidatorTests()
    {
        context = TestingContext.Create();
        validator = new BranchOfficeValidator(new UnitOfWork(TestingContext.Create(), TestingContext.Mapper));

        context.Set<BranchOffice>().Add(branchOffice = ObjectsFactory.CreateBranchOffice());
        context.SaveChanges();
    }

    #region CanCreate(BranchOfficeView view)

    [Fact]
    public void CanCreate_InvalidState_ReturnsFalse()
    {
        validator.ModelState.AddModelError("Test", "Test");

        Assert.False(validator.CanCreate(ObjectsFactory.CreateBranchOfficeView(1)));
    }

    [Fact]
    public void CanCreate_ValidBranchOffice()
    {
        Assert.True(validator.CanCreate(ObjectsFactory.CreateBranchOfficeView(1)));
        Assert.Empty(validator.ModelState);
        Assert.Empty(validator.Alerts);
    }

    #endregion CanCreate(BranchOfficeView view)

    #region CanEdit(BranchOfficeView view)

    [Fact]
    public void CanEdit_InvalidState_ReturnsFalse()
    {
        validator.ModelState.AddModelError("Test", "Test");

        Assert.False(validator.CanEdit(ObjectsFactory.CreateBranchOfficeView(branchOffice.Id)));
    }

    [Fact]
    public void CanEdit_ValidBranchOffice()
    {
        Assert.True(validator.CanEdit(ObjectsFactory.CreateBranchOfficeView(branchOffice.Id)));
        Assert.Empty(validator.ModelState);
        Assert.Empty(validator.Alerts);
    }

    #endregion CanEdit(BranchOfficeView view)
}
