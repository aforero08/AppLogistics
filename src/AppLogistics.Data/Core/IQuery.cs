using System;
using System.Linq;
using System.Linq.Expressions;

namespace AppLogistics.Data.Core;

public interface IQuery<TModel> : IQueryable<TModel>
{
    IQuery<TModel> Where(Expression<Func<TModel, bool>> predicate);

    IQueryable<TView> To<TView>();
}
