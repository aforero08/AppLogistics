using AppLogistics.Objects;
using System.Linq;

namespace AppLogistics.Services;

public interface IBranchOfficeService : IService
{
    TView Get<TView>(int id) where TView : BaseView;
    IQueryable<BranchOfficeView> GetViews();

    void Create(BranchOfficeView view);
    void Edit(BranchOfficeView view);
    void Delete(int id);
}
