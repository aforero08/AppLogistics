using AppLogistics.Data.Core;
using AppLogistics.Objects;
using System.Linq;

namespace AppLogistics.Services;

public class ClientService : BaseService, IClientService
{
    public ClientService(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
    }

    public TView Get<TView>(int id) where TView : BaseView
    {
        return UnitOfWork.GetAs<Client, TView>(id);
    }

    public IQueryable<ClientView> GetViews()
    {
        return UnitOfWork
            .Select<Client>()
            .To<ClientView>()
            .OrderByDescending(client => client.Id);
    }

    public void Create(ClientCreateEditView view)
    {
        Client client = UnitOfWork.To<Client>(view);

        UnitOfWork.Insert(client);
        UnitOfWork.Commit();
    }

    public void Edit(ClientCreateEditView view)
    {
        Client client = UnitOfWork.To<Client>(view);

        UnitOfWork.Update(client);
        UnitOfWork.Commit();
    }

    public void Delete(int id)
    {
        UnitOfWork.Delete<Client>(id);
        UnitOfWork.Commit();
    }
}
