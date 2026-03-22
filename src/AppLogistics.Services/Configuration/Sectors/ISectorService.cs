using AppLogistics.Objects;
using System.Linq;

namespace AppLogistics.Services;

public interface ISectorService : IService
{
    TView Get<TView>(int id) where TView : BaseView;
    IQueryable<SectorView> GetViews();

    void Create(SectorView view);
    void Edit(SectorView view);
    void Delete(int id);
}
