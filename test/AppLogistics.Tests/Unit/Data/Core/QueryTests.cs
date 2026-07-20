using AppLogistics.Tests;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace AppLogistics.Data.Core.Tests;

public class QueryTests
{
    private DbContext context;
    private Query<TestModel> select;

    public QueryTests()
    {
        context = TestFixture.Create();
        select = new Query<TestModel>(context.Set<TestModel>(), TestFixture.Mapper.ConfigurationProvider);

        context.RemoveRange(context.Set<TestModel>());
        context.Add(ObjectsFactory.CreateTestModel());
        context.SaveChanges();
    }

    #region ElementType

    [Fact]
    public void ElementType_IsModelType()
    {
        object actual = (select as IQueryable).ElementType;
        object expected = typeof(TestModel);

        Assert.Same(expected, actual);
    }

    #endregion ElementType

    #region Expression

    [Fact]
    public void Expression_IsSetsExpression()
    {
        DbSet<TestModel> set = Substitute.For<DbSet<TestModel>, IQueryable>();
        DbContext mockContext = Substitute.For<DbContext>();
        ((IQueryable)set).Expression.Returns(Expression.Empty());
        mockContext.Set<TestModel>().Returns(set);

        select = new Query<TestModel>(mockContext.Set<TestModel>(), TestFixture.Mapper.ConfigurationProvider);

        object actual = ((IQueryable)select).Expression;
        object expected = ((IQueryable)set).Expression;

        Assert.Same(expected, actual);
    }

    #endregion Expression

    #region Provider

    [Fact]
    public void Provider_IsSetsProvider()
    {
        object expected = (context.Set<TestModel>() as IQueryable).Provider;
        object actual = (select as IQueryable).Provider;

        Assert.Same(expected, actual);
    }

    #endregion Provider

    #region Select<TResult>(Expression<Func<TModel, TResult>> selector)

    [Fact]
    public void Select_Selects()
    {
        IEnumerable<int> expected = context.Set<TestModel>().Select(model => model.Id);
        IEnumerable<int> actual = select.Select(model => model.Id);

        Assert.Equal(expected, actual);
    }

    #endregion Select<TResult>(Expression<Func<TModel, TResult>> selector)

    #region Where(Expression<Func<TModel, Boolean>> predicate)

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Where_Filters(bool predicate)
    {
        IEnumerable<TestModel> expected = context.Set<TestModel>().Where(model => predicate);
        IEnumerable<TestModel> actual = select.Where(model => predicate);

        Assert.Equal(expected, actual);
    }

    #endregion Where(Expression<Func<TModel, Boolean>> predicate)

    #region To<TView>()

    [Fact]
    public void To_ProjectsSet()
    {
        IEnumerable<int> expected = context.Set<TestModel>().ProjectTo<TestView>(TestFixture.Mapper.ConfigurationProvider).Select(view => view.Id).ToArray();
        IEnumerable<int> actual = select.To<TestView>().Select(view => view.Id).ToArray();

        Assert.Equal(expected, actual);
    }

    #endregion To<TView>()

    #region GetEnumerator()

    [Fact]
    public void GetEnumerator_ReturnsSetEnumerator()
    {
        IEnumerable<TestModel> expected = context.Set<TestModel>();
        IEnumerable<TestModel> actual = select.ToArray();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetEnumerator_ReturnsSameEnumerator()
    {
        IEnumerable<TestModel> expected = context.Set<TestModel>();
        IEnumerable<TestModel> actual = select;

        Assert.Equal(expected, actual);
    }

    #endregion GetEnumerator()
}
