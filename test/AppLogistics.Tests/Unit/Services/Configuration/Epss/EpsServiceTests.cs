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

public class EpsServiceTests
{
    private EpsService service;
    private DbContext context;
    private Eps eps;

    public EpsServiceTests()
    {
        context = TestFixture.Create().Drop();
        service = new EpsService(new UnitOfWork(TestFixture.Create(), TestFixture.Mapper));

        context.Set<Eps>().Add(eps = ObjectsFactory.CreateEps());
        context.SaveChanges();
    }

    #region Get<TView>(String id)

    [Fact]
    public void Get_ReturnsViewById()
    {
        EpsView actual = service.Get<EpsView>(eps.Id);
        EpsView expected = TestFixture.Mapper.Map<EpsView>(eps);

        Assert.Equal(expected.CreationDate, actual.CreationDate);
        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.Nit, actual.Nit);
        Assert.Equal(expected.Id, actual.Id);
    }

    #endregion Get<TView>(String id)

    #region GetViews()

    [Fact]
    public void GetViews_ReturnsEpsViews()
    {
        EpsView[] actual = service.GetViews().ToArray();
        EpsView[] expected = context
            .Set<Eps>()
            .ProjectTo<EpsView>(TestFixture.Mapper.ConfigurationProvider)
            .OrderByDescending(view => view.CreationDate)
            .ToArray();

        for (int i = 0; i < expected.Length || i < actual.Length; i++)
        {
            Assert.Equal(expected[i].CreationDate, actual[i].CreationDate);
            Assert.Equal(expected[i].Name, actual[i].Name);
            Assert.Equal(expected[i].Nit, actual[i].Nit);
            Assert.Equal(expected[i].Id, actual[i].Id);
        }
    }

    #endregion GetViews()

    #region Create(EpsView view)

    [Fact]
    public void Create_Eps()
    {
        EpsView view = ObjectsFactory.CreateEpsView(1);
        view.Id = 0;

        service.Create(view);

        Eps actual = context.Set<Eps>().AsNoTracking().Single(model => model.Id != eps.Id);
        EpsView expected = view;

        Assert.Equal(expected.CreationDate, actual.CreationDate);
        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.Nit, actual.Nit);
    }

    #endregion Create(EpsView view)

    #region Edit(EpsView view)

    [Fact]
    public void Edit_Eps()
    {
        EpsView view = ObjectsFactory.CreateEpsView(eps.Id);
        view.Name = "Name0";
        view.Nit = "Nit0";

        service.Edit(view);

        Eps actual = context.Set<Eps>().AsNoTracking().Single();
        Eps expected = eps;

        Assert.Equal(expected.CreationDate, actual.CreationDate);
        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.Nit, actual.Nit);
        Assert.Equal(expected.Id, actual.Id);
    }

    #endregion Edit(EpsView view)

    #region Delete(String id)

    [Fact]
    public void Delete_Eps()
    {
        service.Delete(eps.Id);

        Assert.Empty(context.Set<Eps>());
    }

    #endregion Delete(String id)
}
