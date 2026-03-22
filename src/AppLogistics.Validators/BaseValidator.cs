using AppLogistics.Components.Notifications;
using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Resources;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq.Expressions;

namespace AppLogistics.Validators;

public abstract class BaseValidator : IValidator
{
    public ModelStateDictionary ModelState { get; set; }
    public int CurrentAccountId { get; set; }
    public Alerts Alerts { get; set; }

    protected IUnitOfWork UnitOfWork { get; }

    protected BaseValidator(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;

        ModelState = new ModelStateDictionary();
        Alerts = new Alerts();
    }

    protected bool IsSpecified<TView>(TView view, Expression<Func<TView, object>> property) where TView : BaseView
    {
        bool isSpecified = property.Compile().Invoke(view) != null;

        if (!isSpecified)
        {
            if (property.Body is UnaryExpression unary)
            {
                ModelState.AddModelError(property, Validation.For("Required", Resource.ForProperty(unary.Operand)));
            }
            else
            {
                ModelState.AddModelError(property, Validation.For("Required", Resource.ForProperty(property)));
            }
        }

        return isSpecified;
    }

    public void Dispose()
    {
        UnitOfWork.Dispose();
    }
}
