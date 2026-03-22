using AppLogistics.Objects;
using System.Linq;

namespace AppLogistics.Services;

public interface IEthnicGroupService : IService
{
    TView Get<TView>(int id) where TView : BaseView;
    IQueryable<EthnicGroupView> GetViews();

    void Create(EthnicGroupView view);
    void Edit(EthnicGroupView view);
    void Delete(int id);
}
