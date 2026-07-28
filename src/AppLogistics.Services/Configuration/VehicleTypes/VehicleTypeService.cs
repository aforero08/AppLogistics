using AppLogistics.Data.Core;
using AppLogistics.Objects;
using System.Linq;

namespace AppLogistics.Services;

public class VehicleTypeService : BaseService, IVehicleTypeService
{
    public VehicleTypeService(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
    }

    public TView Get<TView>(int id) where TView : BaseView
    {
        return UnitOfWork.GetAs<VehicleType, TView>(id);
    }

    public IQueryable<VehicleTypeView> GetViews()
    {
        return UnitOfWork
            .Select<VehicleType>()
            .To<VehicleTypeView>()
            .OrderByDescending(vehicleType => vehicleType.Id);
    }

    public void Create(VehicleTypeView view)
    {
        VehicleType vehicleType = UnitOfWork.To<VehicleType>(view);

        UnitOfWork.Insert(vehicleType);
        UnitOfWork.Commit();
    }

    public void Edit(VehicleTypeView view)
    {
        VehicleType vehicleType = UnitOfWork.To<VehicleType>(view);

        UnitOfWork.Update(vehicleType);
        UnitOfWork.Commit();
    }

    public void Delete(int id)
    {
        UnitOfWork.Delete<VehicleType>(id);
        UnitOfWork.Commit();
    }
}
