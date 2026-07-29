using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Tests;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;

namespace AppLogistics.Services.Tests;

public class EducationLevelServiceTests
{
    private EducationLevelService service;
    private DbContext context;
    private EducationLevel educationLevel;

    public EducationLevelServiceTests()
    {
        context = TestFixture.Create().Drop();
        service = new EducationLevelService(new UnitOfWork(TestFixture.Create(), TestFixture.Mapper));

        context.Set<EducationLevel>().Add(educationLevel = ObjectsFactory.CreateEducationLevel());
        context.SaveChanges();
    }

    #region Get<TView>(String id)

    [Fact]
    public void Get_ReturnsViewById()
    {
        EducationLevelView actual = service.Get<EducationLevelView>(educationLevel.Id);
        EducationLevelView expected = TestFixture.Mapper.Map<EducationLevelView>(educationLevel);

        Assert.Equal(expected.CreationDate, actual.CreationDate);
        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.Id, actual.Id);
    }

    #endregion

    #region GetViews()

    [Fact]
    public void GetViews_ReturnsEducationLevelViews()
    {
        EducationLevelView[] actual = service.GetViews().ToArray();
        EducationLevelView[] expected = context
            .Set<EducationLevel>()
            .ProjectTo<EducationLevelView>(TestFixture.Mapper.ConfigurationProvider)
            .OrderByDescending(view => view.CreationDate)
            .ToArray();

        for (int i = 0; i < expected.Length || i < actual.Length; i++)
        {
                            Assert.Equal(expected[i].CreationDate, actual[i].CreationDate);
            Assert.Equal(expected[i].Name, actual[i].Name);
            Assert.Equal(expected[i].Id, actual[i].Id);
        }
    }

    #endregion

    #region Create(EducationLevelView view)

    [Fact]
    public void Create_EducationLevel()
    {
        EducationLevelView view = ObjectsFactory.CreateEducationLevelView(1);
        view.Id = 0;

        service.Create(view);

        EducationLevel actual = context.Set<EducationLevel>().AsNoTracking().Single(model => model.Id != educationLevel.Id);
        EducationLevelView expected = view;

        Assert.Equal(expected.CreationDate, actual.CreationDate);
        Assert.Equal(expected.Name, actual.Name);
    }

    #endregion

    #region Edit(EducationLevelView view)

    [Fact]
    public void Edit_EducationLevel()
    {
        EducationLevelView view = ObjectsFactory.CreateEducationLevelView(educationLevel.Id);
        view.Name = "Name0";

        service.Edit(view);

        EducationLevel actual = context.Set<EducationLevel>().AsNoTracking().Single();
        EducationLevel expected = educationLevel;

        Assert.Equal(expected.CreationDate, actual.CreationDate);
        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.Id, actual.Id);
    }

    #endregion

    #region Delete(String id)

    [Fact]
    public void Delete_EducationLevel()
    {
        service.Delete(educationLevel.Id);

        Assert.Empty(context.Set<EducationLevel>());
    }

    #endregion
}
