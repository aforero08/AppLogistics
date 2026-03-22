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

public class ActivityServiceTests
{
    private ActivityService service;
    private DbContext context;
    private Activity activity;

    public ActivityServiceTests()
    {
        context = TestingContext.Create();
        service = new ActivityService(new UnitOfWork(TestingContext.Create(), TestingContext.Mapper));

        context.Set<Activity>().Add(activity = ObjectsFactory.CreateActivity());
        context.SaveChanges();
    }

    #region Get<TView>(String id)

    [Fact]
    public void Get_ReturnsViewById()
    {
        ActivityView actual = service.Get<ActivityView>(activity.Id);
        ActivityView expected = TestingContext.Mapper.Map<ActivityView>(activity);

        Assert.Equal(expected.CreationDate, actual.CreationDate);
        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.Id, actual.Id);
    }

    #endregion Get<TView>(String id)

    #region GetViews()

    [Fact]
    public void GetViews_ReturnsActivityViews()
    {
        ActivityView[] actual = service.GetViews().ToArray();
        ActivityView[] expected = context
            .Set<Activity>()
            .ProjectTo<ActivityView>(TestingContext.Mapper.ConfigurationProvider)
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

    #region Create(ActivityView view)

    [Fact]
    public void Create_Activity()
    {
        ActivityView view = ObjectsFactory.CreateActivityView(1);
        view.Id = 0;

        service.Create(view);

        Activity actual = context.Set<Activity>().AsNoTracking().Single(model => model.Id != activity.Id);
        ActivityView expected = view;

        Assert.Equal(expected.CreationDate, actual.CreationDate);
        Assert.Equal(expected.Name, actual.Name);
    }

    #endregion Create(ActivityView view)

    #region Edit(ActivityView view)

    [Fact]
    public void Edit_Activity()
    {
        ActivityView view = ObjectsFactory.CreateActivityView(activity.Id);
        view.Name = "Name0";

        service.Edit(view);

        Activity actual = context.Set<Activity>().AsNoTracking().Single();
        Activity expected = activity;

        Assert.Equal(expected.CreationDate, actual.CreationDate);
        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.Id, actual.Id);
    }

    #endregion Edit(ActivityView view)

    #region Delete(String id)

    [Fact]
    public void Delete_Activity()
    {
        service.Delete(activity.Id);

        Assert.Empty(context.Set<Activity>());
    }

    #endregion Delete(String id)
}
