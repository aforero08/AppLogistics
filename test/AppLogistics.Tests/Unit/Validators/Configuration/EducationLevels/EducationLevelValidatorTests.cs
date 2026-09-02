using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Tests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AppLogistics.Validators.Tests;

public class EducationLevelValidatorTests
{
    private EducationLevelValidator validator;
    private DbContext context;
    private EducationLevel educationLevel;

    public EducationLevelValidatorTests()
    {
        context = TestFixture.Create().Drop();
        validator = new EducationLevelValidator(new UnitOfWork(context, TestFixture.Mapper));

        context.Set<EducationLevel>().Add(educationLevel = ObjectsFactory.CreateEducationLevel());
        context.SaveChanges();
    }

    #region CanCreate(EducationLevelView view)

    [Fact]
    public void CanCreate_InvalidState_ReturnsFalse()
    {
        validator.ModelState.AddModelError("Test", "Test");

        Assert.False(validator.CanCreate(ObjectsFactory.CreateEducationLevelView(1)));
    }

    [Fact]
    public void CanCreate_ValidEducationLevel()
    {
        Assert.True(validator.CanCreate(ObjectsFactory.CreateEducationLevelView(1)));
        Assert.Empty(validator.ModelState);
        Assert.Empty(validator.Alerts);
    }

    #endregion

    #region CanEdit(EducationLevelView view)

    [Fact]
    public void CanEdit_InvalidState_ReturnsFalse()
    {
        validator.ModelState.AddModelError("Test", "Test");

        Assert.False(validator.CanEdit(ObjectsFactory.CreateEducationLevelView(educationLevel.Id)));
    }

    [Fact]
    public void CanEdit_ValidEducationLevel()
    {
        EducationLevelView view = ObjectsFactory.CreateEducationLevelView(educationLevel.Id);
        view.Name = educationLevel.Name;

        Assert.True(validator.CanEdit(view));
        Assert.Empty(validator.ModelState);
        Assert.Empty(validator.Alerts);
    }

    [Fact]
    public void CanEdit_DuplicateName_ReturnsFalse()
    {
        EducationLevel otherEducationLevel = ObjectsFactory.CreateEducationLevel();
        otherEducationLevel.Name = "OtherName";
        context.Set<EducationLevel>().Add(otherEducationLevel);
        context.SaveChanges();

        EducationLevelView view = ObjectsFactory.CreateEducationLevelView(educationLevel.Id);
        view.Name = otherEducationLevel.Name.ToLowerInvariant();

        Assert.False(validator.CanEdit(view));
        Assert.NotEmpty(validator.Alerts);
    }

    #endregion
}
