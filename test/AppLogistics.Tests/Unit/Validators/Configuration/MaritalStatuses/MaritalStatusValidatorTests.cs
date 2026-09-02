using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Tests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AppLogistics.Validators.Tests;

public class MaritalStatusValidatorTests
{
    private MaritalStatusValidator validator;
    private DbContext context;
    private MaritalStatus maritalStatus;

    public MaritalStatusValidatorTests()
    {
        context = TestFixture.Create().Drop();
        validator = new MaritalStatusValidator(new UnitOfWork(context, TestFixture.Mapper));

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
        MaritalStatusView view = ObjectsFactory.CreateMaritalStatusView(maritalStatus.Id);
        view.Name = maritalStatus.Name;

        Assert.True(validator.CanEdit(view));
        Assert.Empty(validator.ModelState);
        Assert.Empty(validator.Alerts);
    }

    [Fact]
    public void CanEdit_DuplicateName_ReturnsFalse()
    {
        MaritalStatus otherMaritalStatus = ObjectsFactory.CreateMaritalStatus();
        otherMaritalStatus.Name = "OtherName";
        context.Set<MaritalStatus>().Add(otherMaritalStatus);
        context.SaveChanges();

        MaritalStatusView view = ObjectsFactory.CreateMaritalStatusView(maritalStatus.Id);
        view.Name = otherMaritalStatus.Name.ToLowerInvariant();

        Assert.False(validator.CanEdit(view));
        Assert.NotEmpty(validator.Alerts);
    }

    #endregion CanEdit(MaritalStatusView view)
}
