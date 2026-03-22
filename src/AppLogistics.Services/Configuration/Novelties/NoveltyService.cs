using AppLogistics.Data.Core;
using AppLogistics.Objects;
using System.Linq;

namespace AppLogistics.Services;

public class NoveltyService : BaseService, INoveltyService
{
    public NoveltyService(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
    }

    public TView Get<TView>(int id) where TView : BaseView
    {
        return UnitOfWork.GetAs<Novelty, TView>(id);
    }

    public IQueryable<NoveltyView> GetViews()
    {
        return UnitOfWork
            .Select<Novelty>()
            .To<NoveltyView>()
            .OrderByDescending(novelty => novelty.Id);
    }

    public void Create(NoveltyView view)
    {
        Novelty novelty = UnitOfWork.To<Novelty>(view);

        UnitOfWork.Insert(novelty);
        UnitOfWork.Commit();
    }

    public void Edit(NoveltyView view)
    {
        Novelty novelty = UnitOfWork.To<Novelty>(view);

        UnitOfWork.Update(novelty);
        UnitOfWork.Commit();
    }

    public void Delete(int id)
    {
        UnitOfWork.Delete<Novelty>(id);
        UnitOfWork.Commit();
    }
}
