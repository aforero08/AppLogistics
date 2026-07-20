using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Tests;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace AppLogistics.Services.Tests;

public class NoveltyServiceTests
{
    private NoveltyService service;
    private DbContext context;
    private Novelty novelty;

    public NoveltyServiceTests()
    {
        context = TestFixture.Create().Drop();
        service = new NoveltyService(new UnitOfWork(TestFixture.Create(), TestFixture.Mapper));

        context.Set<Novelty>().Add(novelty = ObjectsFactory.CreateNovelty());
        context.SaveChanges();
    }

    #region Get<TView>(String id)

    [Fact]
    public void Get_ReturnsViewById()
    {
        NoveltyView actual = service.Get<NoveltyView>(novelty.Id);
        NoveltyView expected = TestFixture.Mapper.Map<NoveltyView>(novelty);

        Assert.Equal(expected.CreationDate, actual.CreationDate);
        Assert.Equal(expected.Description, actual.Description);
        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.Id, actual.Id);
    }

    #endregion

    #region GetViews()

    [Fact]
    public void GetViews_ReturnsNoveltyViews()
    {
        NoveltyView[] actual = service.GetViews().ToArray();
        NoveltyView[] expected = context
            .Set<Novelty>()
            .ProjectTo<NoveltyView>(TestFixture.Mapper.ConfigurationProvider)
            .OrderByDescending(view => view.CreationDate)
            .ToArray();

        for (int i = 0; i < expected.Length || i < actual.Length; i++)
        {
                            Assert.Equal(expected[i].CreationDate, actual[i].CreationDate);
            Assert.Equal(expected[i].Description, actual[i].Description);
            Assert.Equal(expected[i].Name, actual[i].Name);
            Assert.Equal(expected[i].Id, actual[i].Id);
        }
    }

    #endregion

    #region Create(NoveltyView view)

    [Fact]
    public void Create_Novelty()
    {
        NoveltyView view = ObjectsFactory.CreateNoveltyView(1);
        view.Id = 0;

        service.Create(view);

        Novelty actual = context.Set<Novelty>().AsNoTracking().Single(model => model.Id != novelty.Id);
        NoveltyView expected = view;

        Assert.Equal(expected.CreationDate, actual.CreationDate);
        Assert.Equal(expected.Description, actual.Description);
        Assert.Equal(expected.Name, actual.Name);
    }

    #endregion

    #region Edit(NoveltyView view)

    [Fact]
    public void Edit_Novelty()
    {
        NoveltyView view = ObjectsFactory.CreateNoveltyView(novelty.Id);
        view.Name = "Name0";

        service.Edit(view);

        Novelty actual = context.Set<Novelty>().AsNoTracking().Single();
        Novelty expected = novelty;

        Assert.Equal(expected.CreationDate, actual.CreationDate);
        Assert.Equal(expected.Description, actual.Description);
        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.Id, actual.Id);
    }

    #endregion

    #region Delete(String id)

    [Fact]
    public void Delete_Novelty()
    {
        service.Delete(novelty.Id);

        Assert.Empty(context.Set<Novelty>());
    }

    #endregion
}
