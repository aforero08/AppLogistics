using AppLogistics.Objects;

namespace AppLogistics.Validators;

public interface IRateValidator : IValidator
{
    bool CanCreate(RateCreateEditView view);
    bool CanEdit(RateCreateEditView view);
    bool CanDelete(int id);
}
