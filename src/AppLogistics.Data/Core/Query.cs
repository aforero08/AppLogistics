using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AppLogistics.Data.Core;

public class Query<TModel> : IQuery<TModel> where TModel : class
{
    public Type ElementType => _set.ElementType;
    public Expression Expression => _set.Expression;
    public IQueryProvider Provider => _set.Provider;

    private IQueryable<TModel> _set { get; }
    private IConfigurationProvider _mapper { get; }

    public Query(IQueryable<TModel> set, IConfigurationProvider mapper)
    {
        _set = set;
        _mapper = mapper;
    }

    public IQuery<TResult> Select<TResult>(Expression<Func<TModel, TResult>> selector) where TResult : class
    {
        return new Query<TResult>(_set.Select(selector), _mapper);
    }

    public IQuery<TModel> Where(Expression<Func<TModel, bool>> predicate)
    {
        return new Query<TModel>(_set.Where(predicate), _mapper);
    }

    public IQueryable<TView> To<TView>()
    {
        return _set.AsNoTracking().ProjectTo<TView>(_mapper);
    }

    public IEnumerator<TModel> GetEnumerator() => _set.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
