using AppLogistics.Data.Core;
using AppLogistics.Objects;
using System.Linq;

namespace AppLogistics.Services;

public class EpsService : BaseService, IEpsService
{
    public EpsService(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
    }

    public TView Get<TView>(int id) where TView : BaseView
    {
        return UnitOfWork.GetAs<Eps, TView>(id);
    }

    public IQueryable<EpsView> GetViews()
    {
        return UnitOfWork
            .Select<Eps>()
            .To<EpsView>()
            .OrderByDescending(eps => eps.Id);
    }

    public void Create(EpsView view)
    {
        Eps eps = UnitOfWork.To<Eps>(view);

        UnitOfWork.Insert(eps);
        UnitOfWork.Commit();
    }

    public void Edit(EpsView view)
    {
        Eps eps = UnitOfWork.To<Eps>(view);

        UnitOfWork.Update(eps);
        UnitOfWork.Commit();
    }

    public void Delete(int id)
    {
        UnitOfWork.Delete<Eps>(id);
        UnitOfWork.Commit();
    }
}
