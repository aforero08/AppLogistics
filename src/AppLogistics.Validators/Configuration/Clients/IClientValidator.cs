using AppLogistics.Objects;

namespace AppLogistics.Validators;

public interface IClientValidator : IValidator
{
    bool CanCreate(ClientCreateEditView view);
    bool CanEdit(ClientCreateEditView view);
    bool CanDelete(int id);
}
