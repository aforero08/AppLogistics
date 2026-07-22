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

public class EthnicGroupServiceTests
{
    private EthnicGroupService service;
    private DbContext context;
    private EthnicGroup ethnicGroup;

    public EthnicGroupServiceTests()
    {
        context = TestFixture.Create().Drop();
        service = new EthnicGroupService(new UnitOfWork(TestFixture.Create(), TestFixture.Mapper));

        context.Set<EthnicGroup>().Add(ethnicGroup = ObjectsFactory.CreateEthnicGroup());
        context.SaveChanges();
    }

    #region Get<TView>(String id)

    [Fact]
    public void Get_ReturnsViewById()
    {
        EthnicGroupView actual = service.Get<EthnicGroupView>(ethnicGroup.Id);
        EthnicGroupView expected = TestFixture.Mapper.Map<EthnicGroupView>(ethnicGroup);

        Assert.Equal(expected.CreationDate, actual.CreationDate);
        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.Id, actual.Id);
    }

    #endregion

    #region GetViews()

    [Fact]
    public void GetViews_ReturnsEthnicGroupViews()
    {
        EthnicGroupView[] actual = service.GetViews().ToArray();
        EthnicGroupView[] expected = context
            .Set<EthnicGroup>()
            .ProjectTo<EthnicGroupView>(TestFixture.Mapper.ConfigurationProvider)
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

    #region Create(EthnicGroupView view)

    [Fact]
    public void Create_EthnicGroup()
    {
        EthnicGroupView view = ObjectsFactory.CreateEthnicGroupView(1);
        view.Id = 0;

        service.Create(view);

        EthnicGroup actual = context.Set<EthnicGroup>().AsNoTracking().Single(model => model.Id != ethnicGroup.Id);
        EthnicGroupView expected = view;

        Assert.Equal(expected.CreationDate, actual.CreationDate);
        Assert.Equal(expected.Name, actual.Name);
    }

    #endregion

    #region Edit(EthnicGroupView view)

    [Fact]
    public void Edit_EthnicGroup()
    {
        EthnicGroupView view = ObjectsFactory.CreateEthnicGroupView(ethnicGroup.Id);
        view.Name = "Name0";

        service.Edit(view);

        EthnicGroup actual = context.Set<EthnicGroup>().AsNoTracking().Single();
        EthnicGroup expected = ethnicGroup;

        Assert.Equal(expected.CreationDate, actual.CreationDate);
        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.Id, actual.Id);
    }

    #endregion

    #region Delete(String id)

    [Fact]
    public void Delete_EthnicGroup()
    {
        service.Delete(ethnicGroup.Id);

        Assert.Empty(context.Set<EthnicGroup>());
    }

    #endregion
}
