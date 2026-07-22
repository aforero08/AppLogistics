using AppLogistics.Objects;

namespace AppLogistics.Validators;

public interface IServiceValidator : IValidator
{
    bool CanCreate(ServiceCreateEditView view);
    bool CanEdit(ServiceCreateEditView view);
    bool CanFinalize(int serviceId);
}
