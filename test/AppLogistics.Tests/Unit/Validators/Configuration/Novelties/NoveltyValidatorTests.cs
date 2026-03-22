using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Tests;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace AppLogistics.Validators.Tests;

public class NoveltyValidatorTests
{
    private NoveltyValidator validator;
    private DbContext context;
    private Novelty novelty;

    public NoveltyValidatorTests()
    {
        context = TestingContext.Create();
        validator = new NoveltyValidator(new UnitOfWork(TestingContext.Create(), TestingContext.Mapper));

        context.Set<Novelty>().Add(novelty = ObjectsFactory.CreateNovelty());
        context.SaveChanges();
    }

    #region CanCreate(NoveltyView view)

    [Fact]
    public void CanCreate_InvalidState_ReturnsFalse()
    {
        validator.ModelState.AddModelError("Test", "Test");

        Assert.False(validator.CanCreate(ObjectsFactory.CreateNoveltyView(1)));
    }

    [Fact]
    public void CanCreate_ValidNovelty()
    {
        Assert.True(validator.CanCreate(ObjectsFactory.CreateNoveltyView(1)));
        Assert.Empty(validator.ModelState);
        Assert.Empty(validator.Alerts);
    }

    #endregion

    #region CanEdit(NoveltyView view)

    [Fact]
    public void CanEdit_InvalidState_ReturnsFalse()
    {
        validator.ModelState.AddModelError("Test", "Test");

        Assert.False(validator.CanEdit(ObjectsFactory.CreateNoveltyView(novelty.Id)));
    }

    [Fact]
    public void CanEdit_ValidNovelty()
    {
        Assert.True(validator.CanEdit(ObjectsFactory.CreateNoveltyView(novelty.Id)));
        Assert.Empty(validator.ModelState);
        Assert.Empty(validator.Alerts);
    }

    #endregion
}
