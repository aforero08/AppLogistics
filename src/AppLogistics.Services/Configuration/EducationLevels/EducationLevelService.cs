using AppLogistics.Data.Core;
using AppLogistics.Objects;
using System.Linq;

namespace AppLogistics.Services;

public class EducationLevelService : BaseService, IEducationLevelService
{
    public EducationLevelService(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
    }

    public TView Get<TView>(int id) where TView : BaseView
    {
        return UnitOfWork.GetAs<EducationLevel, TView>(id);
    }

    public IQueryable<EducationLevelView> GetViews()
    {
        return UnitOfWork
            .Select<EducationLevel>()
            .To<EducationLevelView>()
            .OrderByDescending(educationLevel => educationLevel.Id);
    }

    public void Create(EducationLevelView view)
    {
        EducationLevel educationLevel = UnitOfWork.To<EducationLevel>(view);

        UnitOfWork.Insert(educationLevel);
        UnitOfWork.Commit();
    }

    public void Edit(EducationLevelView view)
    {
        EducationLevel educationLevel = UnitOfWork.To<EducationLevel>(view);

        UnitOfWork.Update(educationLevel);
        UnitOfWork.Commit();
    }

    public void Delete(int id)
    {
        UnitOfWork.Delete<EducationLevel>(id);
        UnitOfWork.Commit();
    }
}
