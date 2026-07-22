using AppLogistics.Objects;
using System.Linq;

namespace AppLogistics.Services;

public interface IClientService : IService
{
    TView Get<TView>(int id) where TView : BaseView;
    IQueryable<ClientView> GetViews();

    void Create(ClientCreateEditView view);
    void Edit(ClientCreateEditView view);
    void Delete(int id);
}
