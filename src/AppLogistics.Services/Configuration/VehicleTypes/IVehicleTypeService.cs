using AppLogistics.Objects;
using System.Linq;

namespace AppLogistics.Services;

public interface IVehicleTypeService : IService
{
    TView Get<TView>(int id) where TView : BaseView;
    IQueryable<VehicleTypeView> GetViews();

    void Create(VehicleTypeView view);
    void Edit(VehicleTypeView view);
    void Delete(int id);
}
