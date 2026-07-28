using AppLogistics.Objects;

namespace AppLogistics.Validators;

public interface IMaritalStatusValidator : IValidator
{
    bool CanCreate(MaritalStatusView view);
    bool CanEdit(MaritalStatusView view);
    bool CanDelete(int id);
}
