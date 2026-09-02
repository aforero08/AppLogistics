using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Tests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AppLogistics.Validators.Tests;

public class EpsValidatorTests
{
    private EpsValidator validator;
    private DbContext context;
    private Eps eps;

    public EpsValidatorTests()
    {
        context = TestFixture.Create().Drop();
        validator = new EpsValidator(new UnitOfWork(context, TestFixture.Mapper));

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
        EpsView view = ObjectsFactory.CreateEpsView(eps.Id);
        view.Name = eps.Name;

        Assert.True(validator.CanEdit(view));
        Assert.Empty(validator.ModelState);
        Assert.Empty(validator.Alerts);
    }

    [Fact]
    public void CanEdit_DuplicateName_ReturnsFalse()
    {
        Eps otherEps = ObjectsFactory.CreateEps();
        otherEps.Name = "OtherName";
        context.Set<Eps>().Add(otherEps);
        context.SaveChanges();

        EpsView view = ObjectsFactory.CreateEpsView(eps.Id);
        view.Name = otherEps.Name.ToLowerInvariant();

        Assert.False(validator.CanEdit(view));
        Assert.NotEmpty(validator.Alerts);
    }

    #endregion CanEdit(EpsView view)
}
