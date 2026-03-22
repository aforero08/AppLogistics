using AppLogistics.Data.Core;
using AppLogistics.Objects;
using System.Linq;

namespace AppLogistics.Services;

public class RateService : BaseService, IRateService
{
    public RateService(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
    }

    public TView Get<TView>(int id) where TView : BaseView
    {
        return UnitOfWork.GetAs<Rate, TView>(id);
    }

    public IQueryable<RateView> GetViews()
    {
        return UnitOfWork
            .Select<Rate>()
            .To<RateView>()
            .OrderByDescending(rate => rate.Id);
    }

    public void Create(RateCreateEditView view)
    {
        Rate rate = UnitOfWork.To<Rate>(view);

        UnitOfWork.Insert(rate);
        UnitOfWork.Commit();
    }

    public void Edit(RateCreateEditView view)
    {
        Rate rate = UnitOfWork.To<Rate>(view);

        UnitOfWork.Update(rate);
        UnitOfWork.Commit();
    }

    public void Delete(int id)
    {
        UnitOfWork.Delete<Rate>(id);
        UnitOfWork.Commit();
    }
}
