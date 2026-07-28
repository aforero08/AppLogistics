using AppLogistics.Data.Core;
using AppLogistics.Objects;

namespace AppLogistics.Validators;

public class BranchOfficeValidator : BaseValidator, IBranchOfficeValidator
{
    public BranchOfficeValidator(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
    }

    public bool CanCreate(BranchOfficeView view)
    {
        return ModelState.IsValid;
    }

    public bool CanEdit(BranchOfficeView view)
    {
        return ModelState.IsValid;
    }

    public bool CanDelete(int id)
    {
        return ModelState.IsValid;
    }
}
