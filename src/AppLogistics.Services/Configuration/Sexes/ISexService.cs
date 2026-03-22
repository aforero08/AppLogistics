using AppLogistics.Objects;
using System.Linq;

namespace AppLogistics.Services;

public interface ISexService : IService
{
    TView Get<TView>(int id) where TView : BaseView;
    IQueryable<SexView> GetViews();

    void Create(SexView view);
    void Edit(SexView view);
    void Delete(int id);
}
