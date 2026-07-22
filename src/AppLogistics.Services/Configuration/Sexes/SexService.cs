using AppLogistics.Data.Core;
using AppLogistics.Objects;
using System.Linq;

namespace AppLogistics.Services;

public class SexService : BaseService, ISexService
{
    public SexService(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
    }

    public TView Get<TView>(int id) where TView : BaseView
    {
        return UnitOfWork.GetAs<Sex, TView>(id);
    }

    public IQueryable<SexView> GetViews()
    {
        return UnitOfWork
            .Select<Sex>()
            .To<SexView>()
            .OrderByDescending(sex => sex.Id);
    }

    public void Create(SexView view)
    {
        Sex sex = UnitOfWork.To<Sex>(view);

        UnitOfWork.Insert(sex);
        UnitOfWork.Commit();
    }

    public void Edit(SexView view)
    {
        Sex sex = UnitOfWork.To<Sex>(view);

        UnitOfWork.Update(sex);
        UnitOfWork.Commit();
    }

    public void Delete(int id)
    {
        UnitOfWork.Delete<Sex>(id);
        UnitOfWork.Commit();
    }
}
