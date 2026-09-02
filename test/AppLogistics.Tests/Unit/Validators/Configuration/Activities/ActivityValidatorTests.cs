using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Tests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AppLogistics.Validators.Tests;

public class ActivityValidatorTests
{
    private ActivityValidator validator;
    private DbContext context;
    private Activity activity;

    public ActivityValidatorTests()
    {
        context = TestFixture.Create().Drop();
        validator = new ActivityValidator(new UnitOfWork(context, TestFixture.Mapper));

        context.Set<Activity>().Add(activity = ObjectsFactory.CreateActivity());
        context.SaveChanges();
    }

    #region CanCreate(ActivityView view)

    [Fact]
    public void CanCreate_InvalidState_ReturnsFalse()
    {
        validator.ModelState.AddModelError("Test", "Test");

        Assert.False(validator.CanCreate(ObjectsFactory.CreateActivityView(1)));
    }

    [Fact]
    public void CanCreate_ValidActivity()
    {
        Assert.True(validator.CanCreate(ObjectsFactory.CreateActivityView(1)));
        Assert.Empty(validator.ModelState);
        Assert.Empty(validator.Alerts);
    }

    #endregion CanCreate(ActivityView view)

    #region CanEdit(ActivityView view)

    [Fact]
    public void CanEdit_InvalidState_ReturnsFalse()
    {
        validator.ModelState.AddModelError("Test", "Test");

        Assert.False(validator.CanEdit(ObjectsFactory.CreateActivityView(activity.Id)));
    }

    [Fact]
    public void CanEdit_ValidActivity()
    {
        ActivityView view = ObjectsFactory.CreateActivityView(activity.Id);
        view.Name = activity.Name;

        Assert.True(validator.CanEdit(view));
        Assert.Empty(validator.ModelState);
        Assert.Empty(validator.Alerts);
    }

    [Fact]
    public void CanEdit_DuplicateName_ReturnsFalse()
    {
        Activity otherActivity = ObjectsFactory.CreateActivity();
        otherActivity.Name = "OtherName";
        context.Set<Activity>().Add(otherActivity);
        context.SaveChanges();

        ActivityView view = ObjectsFactory.CreateActivityView(activity.Id);
        view.Name = otherActivity.Name.ToLowerInvariant();

        Assert.False(validator.CanEdit(view));
        Assert.NotEmpty(validator.Alerts);
    }

    #endregion CanEdit(ActivityView view)
}
