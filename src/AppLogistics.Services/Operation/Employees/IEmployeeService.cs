using AppLogistics.Objects;
using System.Linq;

namespace AppLogistics.Services;

public interface IEmployeeService : IService
{
    TView Get<TView>(int id) where TView : BaseView;
    IQueryable<EmployeeView> GetViews();

    void Create(EmployeeCreateEditView view);
    void Edit(EmployeeCreateEditView view);
    void Delete(int id);
}
