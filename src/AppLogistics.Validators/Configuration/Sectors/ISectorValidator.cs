using AppLogistics.Objects;

namespace AppLogistics.Validators;

public interface ISectorValidator : IValidator
{
    bool CanCreate(SectorView view);
    bool CanEdit(SectorView view);
    bool CanDelete(int id);
}
