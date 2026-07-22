using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Resources;

namespace AppLogistics.Validators;

public class ServiceValidator : BaseValidator, IServiceValidator
{
    public ServiceValidator(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
    }

    public bool CanCreate(ServiceCreateEditView view)
    {
        return ModelState.IsValid 
            && AreRelatedRateAndClient(view.RateId, view.RateClientId) 
            && HasValidVehicleType(view.RateId, view.SpecifyVehicleType, view.VehicleTypeId);
    }

    public bool CanEdit(ServiceCreateEditView view)
    {
        return ModelState.IsValid
            && AreRelatedRateAndClient(view.RateId, view.RateClientId)
            && HasValidVehicleType(view.RateId, view.SpecifyVehicleType, view.VehicleTypeId);
    }

    public bool CanFinalize(int serviceId)
    {
        var service = UnitOfWork.Get<Service>(serviceId);

        if (service.EndDate != null)
        {
            Alerts.AddError(Validation.For<ServiceCreateEditView>("ServiceAlreadyFinalized"));
            return false;
        }

        return true;
    }

    private bool AreRelatedRateAndClient(int rateId, int clientId)
    {
        var rate = UnitOfWork.Get<Rate>(rateId);
        if (rate.ClientId != clientId)
        {
            Alerts.AddError(Validation.For<ServiceCreateEditView>("RateAndClientUnrelated"));
            return false;
        }

        return true;
    }

    private bool HasValidVehicleType(int rateId, bool useSpecificVehicleType, int? vehicleTypeId)
    {
        var rate = UnitOfWork.Get<Rate>(rateId);
        if (rate.VehicleTypeId.HasValue && useSpecificVehicleType && vehicleTypeId.HasValue)
        {
            Alerts.AddError(Validation.For<ServiceCreateEditView>("RateAlreadyHasVehicleId"));
            return false;
        }

        if (useSpecificVehicleType && !vehicleTypeId.HasValue)
        {
            Alerts.AddError(Validation.For<ServiceCreateEditView>("SpecificVehicleIdNotSelected"));
            return false;
        }

        if (!useSpecificVehicleType && vehicleTypeId.HasValue)
        {
            Alerts.AddError(Validation.For<ServiceCreateEditView>("VehicleSelectedWithoutCheckMarked"));
            return false;
        }

        return true;
    }
}
