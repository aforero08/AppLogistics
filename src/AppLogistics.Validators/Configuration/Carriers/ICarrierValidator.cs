using AppLogistics.Objects;

namespace AppLogistics.Validators;

public interface ICarrierValidator : IValidator
{
    bool CanCreate(CarrierView view);
    bool CanEdit(CarrierView view);
    bool CanDelete(int id);
}
