using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Tests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AppLogistics.Validators.Tests;

public class EthnicGroupValidatorTests
{
    private EthnicGroupValidator validator;
    private DbContext context;
    private EthnicGroup ethnicGroup;

    public EthnicGroupValidatorTests()
    {
        context = TestFixture.Create().Drop();
        validator = new EthnicGroupValidator(new UnitOfWork(context, TestFixture.Mapper));

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
        EthnicGroupView view = ObjectsFactory.CreateEthnicGroupView(ethnicGroup.Id);
        view.Name = ethnicGroup.Name;

        Assert.True(validator.CanEdit(view));
        Assert.Empty(validator.ModelState);
        Assert.Empty(validator.Alerts);
    }

    [Fact]
    public void CanEdit_DuplicateName_ReturnsFalse()
    {
        EthnicGroup otherEthnicGroup = ObjectsFactory.CreateEthnicGroup();
        otherEthnicGroup.Name = "OtherName";
        context.Set<EthnicGroup>().Add(otherEthnicGroup);
        context.SaveChanges();

        EthnicGroupView view = ObjectsFactory.CreateEthnicGroupView(ethnicGroup.Id);
        view.Name = otherEthnicGroup.Name.ToLowerInvariant();

        Assert.False(validator.CanEdit(view));
        Assert.NotEmpty(validator.Alerts);
    }

    #endregion
}
