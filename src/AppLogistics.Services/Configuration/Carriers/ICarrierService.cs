using AppLogistics.Objects;
using System.Linq;

namespace AppLogistics.Services;

public interface ICarrierService : IService
{
    TView Get<TView>(int id) where TView : BaseView;
    IQueryable<CarrierView> GetViews();

    void Create(CarrierView view);
    void Edit(CarrierView view);
    void Delete(int id);
}
