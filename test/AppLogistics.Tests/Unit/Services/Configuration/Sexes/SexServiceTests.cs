using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Tests;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;

namespace AppLogistics.Services.Tests;

public class SexServiceTests
{
    private SexService service;
    private DbContext context;
    private Sex sex;

    public SexServiceTests()
    {
        context = TestFixture.Create().Drop();
        service = new SexService(new UnitOfWork(TestFixture.Create(), TestFixture.Mapper));

        context.Set<Sex>().Add(sex = ObjectsFactory.CreateSex());
        context.SaveChanges();
    }

    #region Get<TView>(String id)

    [Fact]
    public void Get_ReturnsViewById()
    {
        SexView actual = service.Get<SexView>(sex.Id);
        SexView expected = TestFixture.Mapper.Map<SexView>(sex);

        Assert.Equal(expected.CreationDate, actual.CreationDate);
        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.Id, actual.Id);
    }

    #endregion

    #region GetViews()

    [Fact]
    public void GetViews_ReturnsSexViews()
    {
        SexView[] actual = service.GetViews().ToArray();
        SexView[] expected = context
            .Set<Sex>()
            .ProjectTo<SexView>(TestFixture.Mapper.ConfigurationProvider)
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

    #region Create(SexView view)

    [Fact]
    public void Create_Sex()
    {
        SexView view = ObjectsFactory.CreateSexView(1);
        view.Id = 0;

        service.Create(view);

        Sex actual = context.Set<Sex>().AsNoTracking().Single(model => model.Id != sex.Id);
        SexView expected = view;

        Assert.Equal(expected.CreationDate, actual.CreationDate);
        Assert.Equal(expected.Name, actual.Name);
    }

    #endregion

    #region Edit(SexView view)

    [Fact]
    public void Edit_Sex()
    {
        SexView view = ObjectsFactory.CreateSexView(sex.Id);
        view.Name = "Name0";

        service.Edit(view);

        Sex actual = context.Set<Sex>().AsNoTracking().Single();
        Sex expected = sex;

        Assert.Equal(expected.CreationDate, actual.CreationDate);
        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.Id, actual.Id);
    }

    #endregion

    #region Delete(String id)

    [Fact]
    public void Delete_Sex()
    {
        service.Delete(sex.Id);

        Assert.Empty(context.Set<Sex>());
    }

    #endregion
}
