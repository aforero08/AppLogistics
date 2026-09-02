using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Tests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AppLogistics.Validators.Tests;

public class AfpValidatorTests
{
    private AfpValidator validator;
    private DbContext context;
    private Afp afp;

    public AfpValidatorTests()
    {
        context = TestFixture.Create().Drop();
        validator = new AfpValidator(new UnitOfWork(context, TestFixture.Mapper));

        context.Set<Afp>().Add(afp = ObjectsFactory.CreateAfp());
        context.SaveChanges();
    }

    #region CanCreate(AfpView view)

    [Fact]
    public void CanCreate_InvalidState_ReturnsFalse()
    {
        validator.ModelState.AddModelError("Test", "Test");

        Assert.False(validator.CanCreate(ObjectsFactory.CreateAfpView(1)));
    }

    [Fact]
    public void CanCreate_ValidAfp()
    {
        Assert.True(validator.CanCreate(ObjectsFactory.CreateAfpView(1)));
        Assert.Empty(validator.ModelState);
        Assert.Empty(validator.Alerts);
    }

    #endregion CanCreate(AfpView view)

    #region CanEdit(AfpView view)

    [Fact]
    public void CanEdit_InvalidState_ReturnsFalse()
    {
        validator.ModelState.AddModelError("Test", "Test");

        Assert.False(validator.CanEdit(ObjectsFactory.CreateAfpView(afp.Id)));
    }

    [Fact]
    public void CanEdit_ValidAfp()
    {
        AfpView view = ObjectsFactory.CreateAfpView(afp.Id);
        view.Name = afp.Name;

        Assert.True(validator.CanEdit(view));
        Assert.Empty(validator.ModelState);
        Assert.Empty(validator.Alerts);
    }

    [Fact]
    public void CanEdit_DuplicateName_ReturnsFalse()
    {
        Afp otherAfp = ObjectsFactory.CreateAfp();
        otherAfp.Name = "OtherName";
        context.Set<Afp>().Add(otherAfp);
        context.SaveChanges();

        AfpView view = ObjectsFactory.CreateAfpView(afp.Id);
        view.Name = otherAfp.Name.ToLowerInvariant();

        Assert.False(validator.CanEdit(view));
        Assert.NotEmpty(validator.Alerts);
    }

    #endregion CanEdit(AfpView view)
}
