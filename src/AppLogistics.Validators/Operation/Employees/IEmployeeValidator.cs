using AppLogistics.Objects;

namespace AppLogistics.Validators
{
    public interface IEmployeeValidator : IValidator
    {
        bool CanCreate(EmployeeCreateEditView view);
        bool CanEdit(EmployeeCreateEditView view);
        bool CanDelete(EmployeeCreateEditView view) ;
    }
}
