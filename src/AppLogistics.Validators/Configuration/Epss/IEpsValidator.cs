using AppLogistics.Objects;

namespace AppLogistics.Validators;

public interface IEpsValidator : IValidator
{
    bool CanCreate(EpsView view);
    bool CanEdit(EpsView view);
    bool CanDelete(int id);
}
