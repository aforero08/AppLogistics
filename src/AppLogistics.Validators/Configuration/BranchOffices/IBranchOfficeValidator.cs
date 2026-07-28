using AppLogistics.Objects;

namespace AppLogistics.Validators;

public interface IBranchOfficeValidator : IValidator
{
    bool CanCreate(BranchOfficeView view);
    bool CanEdit(BranchOfficeView view);
    bool CanDelete(int id);
}
