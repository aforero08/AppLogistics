using AppLogistics.Objects;
using System.Linq;

namespace AppLogistics.Services;

public interface IEpsService : IService
{
    TView Get<TView>(int id) where TView : BaseView;
    IQueryable<EpsView> GetViews();

    void Create(EpsView view);
    void Edit(EpsView view);
    void Delete(int id);
}
