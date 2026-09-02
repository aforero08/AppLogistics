using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Resources;
using System.Linq;

namespace AppLogistics.Validators;

public class EpsValidator : BaseValidator, IEpsValidator
{
    public EpsValidator(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
    }

    public bool CanCreate(EpsView view)
    {
        var alreadyExists = UnitOfWork.Select<Eps>()
            .Where(c => c.Name.ToUpper().Equals(view.Name.ToUpper()))
            .Any();
        
        if (alreadyExists)
        {
            Alerts.AddError(Validation.For<EpsView>("DuplicateName"));
            return false;
        }
        
        return ModelState.IsValid;
    }

    public bool CanEdit(EpsView view)
    {
        var alreadyExists = UnitOfWork.Select<Eps>()
            .Where(c => c.Id != view.Id && c.Name.ToUpper().Equals(view.Name.ToUpper()))
            .Any();
        
        if (alreadyExists)
        {
            Alerts.AddError(Validation.For<EpsView>("DuplicateName"));
            return false;
        }
        
        return ModelState.IsValid;
    }

    public bool CanDelete(int id)
    {
        var hasReferencedEmployees = UnitOfWork.Select<Employee>()
            .Where(c => c.EpsId.Equals(id))
            .Any();

        if (hasReferencedEmployees)
        {
            Alerts.AddError(Validation.For<EpsView>("AssociatedEmployees"));
            return false;
        }

        return ModelState.IsValid;
    }
}
