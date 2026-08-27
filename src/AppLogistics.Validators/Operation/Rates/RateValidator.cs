using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Resources;
using System.Linq;

namespace AppLogistics.Validators;

public class RateValidator : BaseValidator, IRateValidator
{
    public RateValidator(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
    }

    public bool CanCreate(RateCreateEditView view)
    {
        return IsUniqueName(view.Id, view.Name) && ModelState.IsValid;
    }

    public bool CanEdit(RateCreateEditView view)
    {
        return IsUniqueName(view.Id, view.Name) && ModelState.IsValid;
    }

    public bool CanDelete(int id)
    {
        var hasReferencedServices = UnitOfWork.Select<Service>()
            .Where(c => c.RateId.Equals(id))
            .Any();

        if (hasReferencedServices)
        {
            Alerts.AddError(Validation.For<RateCreateEditView>("AssociatedServices"));
            return false;
        }

        return ModelState.IsValid;
    }

    private bool IsUniqueName(int rateId, string rateName)
    {
        var alreadyExists = UnitOfWork.Select<Rate>()
            .Where(r => r.Name ==rateName && r.Id != rateId)
            .Any();

        if (alreadyExists)
        {
            Alerts.AddError(Validation.For<RateCreateEditView>("NotUniqueName"));
            return false;
        }

        return true;
    }
}
