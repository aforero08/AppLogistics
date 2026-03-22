using AppLogistics.Data.Core;
using AppLogistics.Objects;
using System.Linq;

namespace AppLogistics.Services;

public class ActivityService : BaseService, IActivityService
{
    public ActivityService(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
    }

    public TView Get<TView>(int id) where TView : BaseView
    {
        return UnitOfWork.GetAs<Activity, TView>(id);
    }

    public IQueryable<ActivityView> GetViews()
    {
        return UnitOfWork
            .Select<Activity>()
            .To<ActivityView>()
            .OrderByDescending(activity => activity.Id);
    }

    public void Create(ActivityView view)
    {
        Activity activity = UnitOfWork.To<Activity>(view);

        UnitOfWork.Insert(activity);
        UnitOfWork.Commit();
    }

    public void Edit(ActivityView view)
    {
        Activity activity = UnitOfWork.To<Activity>(view);

        UnitOfWork.Update(activity);
        UnitOfWork.Commit();
    }

    public void Delete(int id)
    {
        UnitOfWork.Delete<Activity>(id);
        UnitOfWork.Commit();
    }
}
