using AppLogistics.Data.Core;
using AppLogistics.Objects;
using System.Linq;

namespace AppLogistics.Services;

public class CarrierService : BaseService, ICarrierService
{
    public CarrierService(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
    }

    public TView Get<TView>(int id) where TView : BaseView
    {
        return UnitOfWork.GetAs<Carrier, TView>(id);
    }

    public IQueryable<CarrierView> GetViews()
    {
        return UnitOfWork
            .Select<Carrier>()
            .To<CarrierView>()
            .OrderByDescending(carrier => carrier.Id);
    }

    public void Create(CarrierView view)
    {
        Carrier carrier = UnitOfWork.To<Carrier>(view);

        UnitOfWork.Insert(carrier);
        UnitOfWork.Commit();
    }

    public void Edit(CarrierView view)
    {
        Carrier carrier = UnitOfWork.To<Carrier>(view);

        UnitOfWork.Update(carrier);
        UnitOfWork.Commit();
    }

    public void Delete(int id)
    {
        UnitOfWork.Delete<Carrier>(id);
        UnitOfWork.Commit();
    }
}
