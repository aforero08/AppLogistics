using AppLogistics.Objects;
using System.Linq;

namespace AppLogistics.Services;

public interface IActivityService : IService
{
    TView Get<TView>(int id) where TView : BaseView;
    IQueryable<ActivityView> GetViews();

    void Create(ActivityView view);
    void Edit(ActivityView view);
    void Delete(int id);
}
