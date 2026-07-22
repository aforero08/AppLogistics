using AppLogistics.Data.Core;
using AppLogistics.Objects;
using System.Linq;

namespace AppLogistics.Services;

public class AfpService : BaseService, IAfpService
{
    public AfpService(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
    }

    public TView Get<TView>(int id) where TView : BaseView
    {
        return UnitOfWork.GetAs<Afp, TView>(id);
    }

    public IQueryable<AfpView> GetViews()
    {
        return UnitOfWork
            .Select<Afp>()
            .To<AfpView>()
            .OrderByDescending(afp => afp.Id);
    }

    public void Create(AfpView view)
    {
        Afp afp = UnitOfWork.To<Afp>(view);

        UnitOfWork.Insert(afp);
        UnitOfWork.Commit();
    }

    public void Edit(AfpView view)
    {
        Afp afp = UnitOfWork.To<Afp>(view);

        UnitOfWork.Update(afp);
        UnitOfWork.Commit();
    }

    public void Delete(int id)
    {
        UnitOfWork.Delete<Afp>(id);
        UnitOfWork.Commit();
    }
}
