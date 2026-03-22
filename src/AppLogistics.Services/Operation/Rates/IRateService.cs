using AppLogistics.Objects;
using System.Linq;

namespace AppLogistics.Services;

public interface IRateService : IService
{
    TView Get<TView>(int id) where TView : BaseView;
    IQueryable<RateView> GetViews();

    void Create(RateCreateEditView view);
    void Edit(RateCreateEditView view);
    void Delete(int id);
}
