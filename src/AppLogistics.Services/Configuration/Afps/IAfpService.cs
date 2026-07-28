using AppLogistics.Objects;
using System.Linq;

namespace AppLogistics.Services;

public interface IAfpService : IService
{
    TView Get<TView>(int id) where TView : BaseView;
    IQueryable<AfpView> GetViews();

    void Create(AfpView view);
    void Edit(AfpView view);
    void Delete(int id);
}
