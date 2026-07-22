using AppLogistics.Objects;

namespace AppLogistics.Validators;

public interface IAfpValidator : IValidator
{
    bool CanCreate(AfpView view);
    bool CanEdit(AfpView view);
    bool CanDelete(int id);
}
