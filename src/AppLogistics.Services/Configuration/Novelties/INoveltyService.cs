using AppLogistics.Objects;
using System.Linq;

namespace AppLogistics.Services;

public interface INoveltyService : IService
{
    TView Get<TView>(int id) where TView : BaseView;
    IQueryable<NoveltyView> GetViews();

    void Create(NoveltyView view);
    void Edit(NoveltyView view);
    void Delete(int id);
}
