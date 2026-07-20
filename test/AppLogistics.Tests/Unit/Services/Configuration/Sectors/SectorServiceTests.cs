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

public class SectorServiceTests
{
    private SectorService service;
    private DbContext context;
    private Sector sector;

    public SectorServiceTests()
    {
        context = TestFixture.Create().Drop();
        service = new SectorService(new UnitOfWork(TestFixture.Create(), TestFixture.Mapper));

        context.Set<Sector>().Add(sector = ObjectsFactory.CreateSector());
        context.SaveChanges();
    }

    #region Get<TView>(String id)

    [Fact]
    public void Get_ReturnsViewById()
    {
        SectorView actual = service.Get<SectorView>(sector.Id);
        SectorView expected = TestFixture.Mapper.Map<SectorView>(sector);

        Assert.Equal(expected.CreationDate, actual.CreationDate);
        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.Id, actual.Id);
    }

    #endregion

    #region GetViews()

    [Fact]
    public void GetViews_ReturnsSectorViews()
    {
        SectorView[] actual = service.GetViews().ToArray();
        SectorView[] expected = context
            .Set<Sector>()
            .ProjectTo<SectorView>(TestFixture.Mapper.ConfigurationProvider)
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

    #region Create(SectorView view)

    [Fact]
    public void Create_Sector()
    {
        SectorView view = ObjectsFactory.CreateSectorView(1);
        view.Id = 0;

        service.Create(view);

        Sector actual = context.Set<Sector>().AsNoTracking().Single(model => model.Id != sector.Id);
        SectorView expected = view;

        Assert.Equal(expected.CreationDate, actual.CreationDate);
        Assert.Equal(expected.Name, actual.Name);
    }

    #endregion

    #region Edit(SectorView view)

    [Fact]
    public void Edit_Sector()
    {
        SectorView view = ObjectsFactory.CreateSectorView(sector.Id);
        view.Name = "Name0";

        service.Edit(view);

        Sector actual = context.Set<Sector>().AsNoTracking().Single();
        Sector expected = sector;

        Assert.Equal(expected.CreationDate, actual.CreationDate);
        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.Id, actual.Id);
    }

    #endregion

    #region Delete(String id)

    [Fact]
    public void Delete_Sector()
    {
        service.Delete(sector.Id);

        Assert.Empty(context.Set<Sector>());
    }

    #endregion
}
