using AppLogistics.Objects;

namespace AppLogistics.Validators;

public interface IActivityValidator : IValidator
{
    bool CanCreate(ActivityView view);
    bool CanEdit(ActivityView view);
    bool CanDelete(int id);
}
