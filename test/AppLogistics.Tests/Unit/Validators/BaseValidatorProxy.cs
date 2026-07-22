using AppLogistics.Data.Core;
using AppLogistics.Objects;
using System;
using System.Linq.Expressions;

namespace AppLogistics.Validators.Tests;

public class BaseValidatorProxy : BaseValidator
{
    public BaseValidatorProxy(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
    }

    public bool BaseIsSpecified<TView>(TView view, Expression<Func<TView, object>> property) where TView : BaseView
    {
        return IsSpecified(view, property);
    }
}
