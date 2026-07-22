using AppLogistics.Services;
using AppLogistics.Validators;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AppLogistics.Controllers;

public abstract class ValidatedController<TValidator, TService> : ServicedController<TService>
    where TValidator : IValidator
    where TService : IService
{
    public TValidator Validator { get; }

    protected ValidatedController(TValidator validator, TService service)
        : base(service)
    {
        Validator = validator;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);

        Validator.CurrentAccountId = Service.CurrentAccountId;
        Validator.ModelState = ModelState;
        Validator.Alerts = Alerts;
    }

    protected override void Dispose(bool disposing)
    {
        Validator.Dispose();

        base.Dispose(disposing);
    }
}
