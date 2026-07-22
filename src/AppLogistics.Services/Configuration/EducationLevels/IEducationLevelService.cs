using AppLogistics.Objects;
using System.Linq;

namespace AppLogistics.Services;

public interface IEducationLevelService : IService
{
    TView Get<TView>(int id) where TView : BaseView;
    IQueryable<EducationLevelView> GetViews();

    void Create(EducationLevelView view);
    void Edit(EducationLevelView view);
    void Delete(int id);
}
