using AppLogistics.Data.Core;
using AppLogistics.Objects;
using System.Linq;

namespace AppLogistics.Services;

public class EmployeeService : BaseService, IEmployeeService
{
    public EmployeeService(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
    }

    public TView Get<TView>(int id) where TView : BaseView
    {
        return UnitOfWork.GetAs<Employee, TView>(id);
    }

    public IQueryable<EmployeeView> GetViews()
    {
        return UnitOfWork
            .Select<Employee>()
            .To<EmployeeView>()
            .OrderByDescending(employee => employee.Id);
    }

    public void Create(EmployeeCreateEditView view)
    {
        Employee employee = UnitOfWork.To<Employee>(view);

        UnitOfWork.Insert(employee);
        UnitOfWork.Commit();
    }

    public void Edit(EmployeeCreateEditView view)
    {
        Employee employee = UnitOfWork.To<Employee>(view);

        UnitOfWork.Update(employee);
        UnitOfWork.Commit();
    }

    public void Delete(int id)
    {
        UnitOfWork.Delete<Employee>(id);
        UnitOfWork.Commit();
    }
}
