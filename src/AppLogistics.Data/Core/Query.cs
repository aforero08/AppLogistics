using AppLogistics.Mapping; // for ProjectTo extension
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AppLogistics.Data.Core
{
    public class Query<TModel> : IQuery<TModel>
    {
        public Type ElementType => _set.ElementType;
        public Expression Expression => _set.Expression;
        public IQueryProvider Provider => _set.Provider;

        private readonly IQueryable<TModel> _set;

        public Query(IQueryable<TModel> set)
        {
            _set = set;
        }

        public IQuery<TResult> Select<TResult>(Expression<Func<TModel, TResult>> selector)
        {
            return new Query<TResult>(_set.Select(selector));
        }

        public IQuery<TModel> Where(Expression<Func<TModel, bool>> predicate)
        {
            return new Query<TModel>(_set.Where(predicate));
        }

        public IQueryable<TView> To<TView>()
        {
            return _set.ProjectTo<TView>();
        }

        public IEnumerator<TModel> GetEnumerator() => _set.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
