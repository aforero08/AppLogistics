using AppLogistics.Objects;

namespace AppLogistics.Validators;

public interface ISexValidator : IValidator
{
    bool CanCreate(SexView view);
    bool CanEdit(SexView view);
    bool CanDelete(int id);
}
