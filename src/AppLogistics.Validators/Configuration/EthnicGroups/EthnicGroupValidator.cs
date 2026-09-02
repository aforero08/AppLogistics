using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Resources;
using System.Linq;

namespace AppLogistics.Validators;

public class EthnicGroupValidator : BaseValidator, IEthnicGroupValidator
{
    public EthnicGroupValidator(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
    }

    public bool CanCreate(EthnicGroupView view)
    {
        var alreadyExists = UnitOfWork.Select<EthnicGroup>()
            .Where(c => c.Name.ToUpper().Equals(view.Name.ToUpper()))
            .Any();
        
        if (alreadyExists)
        {
            Alerts.AddError(Validation.For<EthnicGroupView>("DuplicateName"));
            return false;
        }
        
        return ModelState.IsValid;
    }

    public bool CanEdit(EthnicGroupView view)
    {
        var alreadyExists = UnitOfWork.Select<EthnicGroup>()
            .Where(c => c.Id != view.Id && c.Name.ToUpper().Equals(view.Name.ToUpper()))
            .Any();
        
        if (alreadyExists)
        {
            Alerts.AddError(Validation.For<EthnicGroupView>("DuplicateName"));
            return false;
        }
        
        return ModelState.IsValid;
    }

    public bool CanDelete(int id)
    {
        var hasReferencedEmployees = UnitOfWork.Select<Employee>()
            .Where(c => c.EthnicGroupId.Equals(id))
            .Any();

        if (hasReferencedEmployees)
        {
            Alerts.AddError(Validation.For<EthnicGroupView>("AssociatedEmployees"));
            return false;
        }

        return ModelState.IsValid;
    }
}
