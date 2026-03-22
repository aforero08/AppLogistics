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

public class BranchOfficeServiceTests
{
    private BranchOfficeService service;
    private DbContext context;
    private BranchOffice branchOffice;

    public BranchOfficeServiceTests()
    {
        context = TestingContext.Create();
        service = new BranchOfficeService(new UnitOfWork(TestingContext.Create(), TestingContext.Mapper));

        context.Set<BranchOffice>().Add(branchOffice = ObjectsFactory.CreateBranchOffice());
        context.SaveChanges();
    }

    #region Get<TView>(String id)

    [Fact]
    public void Get_ReturnsViewById()
    {
        BranchOfficeView actual = service.Get<BranchOfficeView>(branchOffice.Id);
        BranchOfficeView expected = TestingContext.Mapper.Map<BranchOfficeView>(branchOffice);

        Assert.Equal(expected.CreationDate, actual.CreationDate);
        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.Id, actual.Id);
    }

    #endregion Get<TView>(String id)

    #region GetViews()

    [Fact]
    public void GetViews_ReturnsBranchOfficeViews()
    {
        BranchOfficeView[] actual = service.GetViews().ToArray();
        BranchOfficeView[] expected = context
            .Set<BranchOffice>()
            .ProjectTo<BranchOfficeView>(TestingContext.Mapper.ConfigurationProvider)
            .OrderByDescending(view => view.CreationDate)
            .ToArray();

        for (int i = 0; i < expected.Length || i < actual.Length; i++)
        {
            Assert.Equal(expected[i].CreationDate, actual[i].CreationDate);
            Assert.Equal(expected[i].Name, actual[i].Name);
            Assert.Equal(expected[i].Id, actual[i].Id);
        }
    }

    #endregion GetViews()

    #region Create(BranchOfficeView view)

    [Fact]
    public void Create_BranchOffice()
    {
        BranchOfficeView view = ObjectsFactory.CreateBranchOfficeView(1);
        view.Id = 0;

        service.Create(view);

        BranchOffice actual = context.Set<BranchOffice>().AsNoTracking().Single(model => model.Id != branchOffice.Id);
        BranchOfficeView expected = view;

        Assert.Equal(expected.CreationDate, actual.CreationDate);
        Assert.Equal(expected.Name, actual.Name);
    }

    #endregion Create(BranchOfficeView view)

    #region Edit(BranchOfficeView view)

    [Fact]
    public void Edit_BranchOffice()
    {
        BranchOfficeView view = ObjectsFactory.CreateBranchOfficeView(branchOffice.Id);
        view.Name = "Name0";

        service.Edit(view);

        BranchOffice actual = context.Set<BranchOffice>().AsNoTracking().Single();
        BranchOffice expected = branchOffice;

        Assert.Equal(expected.CreationDate, actual.CreationDate);
        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.Id, actual.Id);
    }

    #endregion Edit(BranchOfficeView view)

    #region Delete(String id)

    [Fact]
    public void Delete_BranchOffice()
    {
        service.Delete(branchOffice.Id);

        Assert.Empty(context.Set<BranchOffice>());
    }

    #endregion Delete(String id)
}
