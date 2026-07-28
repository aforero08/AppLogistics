using AppLogistics.Objects;

namespace AppLogistics.Validators;

public interface IRoleValidator : IValidator
{
    bool CanCreate(RoleView view);

    bool CanEdit(RoleView view);
}
