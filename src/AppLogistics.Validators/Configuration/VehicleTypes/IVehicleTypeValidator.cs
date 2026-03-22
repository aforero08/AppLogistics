using AppLogistics.Objects;

namespace AppLogistics.Validators;

public interface IVehicleTypeValidator : IValidator
{
    bool CanCreate(VehicleTypeView view);
    bool CanEdit(VehicleTypeView view);
    bool CanDelete(int id);
}
