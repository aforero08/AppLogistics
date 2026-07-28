using AppLogistics.Data.Core;
using AppLogistics.Objects;
using System.Linq;

namespace AppLogistics.Services;

public class MaritalStatusService : BaseService, IMaritalStatusService
{
    public MaritalStatusService(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
    }

    public TView Get<TView>(int id) where TView : BaseView
    {
        return UnitOfWork.GetAs<MaritalStatus, TView>(id);
    }

    public IQueryable<MaritalStatusView> GetViews()
    {
        return UnitOfWork
            .Select<MaritalStatus>()
            .To<MaritalStatusView>()
            .OrderByDescending(maritalStatus => maritalStatus.Id);
    }

    public void Create(MaritalStatusView view)
    {
        MaritalStatus maritalStatus = UnitOfWork.To<MaritalStatus>(view);

        UnitOfWork.Insert(maritalStatus);
        UnitOfWork.Commit();
    }

    public void Edit(MaritalStatusView view)
    {
        MaritalStatus maritalStatus = UnitOfWork.To<MaritalStatus>(view);

        UnitOfWork.Update(maritalStatus);
        UnitOfWork.Commit();
    }

    public void Delete(int id)
    {
        UnitOfWork.Delete<MaritalStatus>(id);
        UnitOfWork.Commit();
    }
}
