using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Resources;
using System.Linq;

namespace AppLogistics.Validators;

public class CarrierValidator : BaseValidator, ICarrierValidator
{
    public CarrierValidator(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
    }

    public bool CanCreate(CarrierView view)
    {
        var alreadyExists = UnitOfWork.Select<Carrier>()
            .Where(a => a.Name.ToUpper().Equals(view.Name.ToUpper()))
            .Any();

        if (alreadyExists)
        {
            Alerts.AddError(Validation.For<CarrierView>("DuplicateName"));
            return false;
        }

        return ModelState.IsValid;
    }

    public bool CanEdit(CarrierView view)
    {
        var alreadyExists = UnitOfWork.Select<Carrier>()
            .Where(a => a.Id != view.Id && a.Name.ToUpper().Equals(view.Name.ToUpper()))
            .Any();

        if (alreadyExists)
        {
            Alerts.AddError(Validation.For<CarrierView>("DuplicateName"));
            return false;
        }

        return ModelState.IsValid;
    }

    public bool CanDelete(int id)
    {
        var hasReferencedServices = UnitOfWork.Select<Service>()
            .Where(c => c.CarrierId.Equals(id))
            .Any();

        if (hasReferencedServices)
        {
            Alerts.AddError(Validation.For<CarrierView>("AssociatedServices"));
            return false;
        }

        return ModelState.IsValid;
    }
}
