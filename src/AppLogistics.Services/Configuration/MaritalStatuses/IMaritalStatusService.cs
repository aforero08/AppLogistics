using AppLogistics.Objects;
using System.Linq;

namespace AppLogistics.Services;

public interface IMaritalStatusService : IService
{
    TView Get<TView>(int id) where TView : BaseView;
    IQueryable<MaritalStatusView> GetViews();

    void Create(MaritalStatusView view);
    void Edit(MaritalStatusView view);
    void Delete(int id);
}
