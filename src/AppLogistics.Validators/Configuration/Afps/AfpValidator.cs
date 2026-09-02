using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Resources;
using System.Linq;

namespace AppLogistics.Validators;

public class AfpValidator : BaseValidator, IAfpValidator
{
    public AfpValidator(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
    }

    public bool CanCreate(AfpView view)
    {
        var alreadyExists = UnitOfWork.Select<Afp>()
            .Where(a => a.Name.ToUpper().Equals(view.Name.ToUpper()))
            .Any();

        if (alreadyExists)
        {
            Alerts.AddError(Validation.For<AfpView>("DuplicateName"));
            return false;
        }

        return ModelState.IsValid;
    }

    public bool CanEdit(AfpView view)
    {
        var alreadyExists = UnitOfWork.Select<Afp>()
            .Where(a => a.Id != view.Id && a.Name.ToUpper().Equals(view.Name.ToUpper()))
            .Any();

        if (alreadyExists)
        {
            Alerts.AddError(Validation.For<AfpView>("DuplicateName"));
            return false;
        }

        return ModelState.IsValid;
    }

    public bool CanDelete(int id)
    {
        var hasReferencedEmployees = UnitOfWork.Select<Employee>()
            .Where(c => c.AfpId.Equals(id))
            .Any();

        if (hasReferencedEmployees)
        {
            Alerts.AddError(Validation.For<AfpView>("AssociatedEmployees"));
            return false;
        }

        return ModelState.IsValid;
    }
}
