using AppLogistics.Objects;
using System.Linq;

namespace AppLogistics.Services;

public interface IServiceService : IService
{
    TView Get<TView>(int id) where TView : ServiceView;
    TView GetEdit<TView>(int id) where TView : ServiceCreateEditView;
    IQueryable<ServiceView> GetViews();

    void Create(ServiceCreateEditView view);
    void Edit(ServiceCreateEditView view);
    void Delete(int id);
    void Finalize(int id);
}
