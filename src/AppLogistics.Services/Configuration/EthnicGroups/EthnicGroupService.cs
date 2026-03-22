using AppLogistics.Data.Core;
using AppLogistics.Objects;
using System.Linq;

namespace AppLogistics.Services;

public class EthnicGroupService : BaseService, IEthnicGroupService
{
    public EthnicGroupService(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
    }

    public TView Get<TView>(int id) where TView : BaseView
    {
        return UnitOfWork.GetAs<EthnicGroup, TView>(id);
    }

    public IQueryable<EthnicGroupView> GetViews()
    {
        return UnitOfWork
            .Select<EthnicGroup>()
            .To<EthnicGroupView>()
            .OrderByDescending(ethnicGroup => ethnicGroup.Id);
    }

    public void Create(EthnicGroupView view)
    {
        EthnicGroup ethnicGroup = UnitOfWork.To<EthnicGroup>(view);

        UnitOfWork.Insert(ethnicGroup);
        UnitOfWork.Commit();
    }

    public void Edit(EthnicGroupView view)
    {
        EthnicGroup ethnicGroup = UnitOfWork.To<EthnicGroup>(view);

        UnitOfWork.Update(ethnicGroup);
        UnitOfWork.Commit();
    }

    public void Delete(int id)
    {
        UnitOfWork.Delete<EthnicGroup>(id);
        UnitOfWork.Commit();
    }
}
