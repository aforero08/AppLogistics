using AppLogistics.Objects;
using System.Linq;

namespace AppLogistics.Services
{
    public interface ISectorService : IService
    {
        TView Get<TView>(int id) where TView : BaseView;
        IQueryable<SectorView> GetViews();

        void Create(SectorCreateEditView view);
        void Edit(SectorCreateEditView view);
        void Delete(int id);
    }
}
