using AppLogistics.Objects;

namespace AppLogistics.Validators
{
    public interface ISectorValidator : IValidator
    {
        bool CanCreate(SectorCreateEditView view);
        bool CanEdit(SectorCreateEditView view);
        bool CanDelete(int id);
    }
}
