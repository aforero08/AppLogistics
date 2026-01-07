using AppLogistics.Components.Notifications;
using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Resources;
using System.Linq;

namespace AppLogistics.Validators
{
    public class EmployeeValidator : BaseValidator, IEmployeeValidator
    {
        public EmployeeValidator(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public bool CanCreate(EmployeeCreateEditView view)
        {
            return IsUniqueDocumentNumber(view.Id, view.DocumentNumber)
                && IsUniqueInternalCode(view.Id, view.InternalCode)
                && ModelState.IsValid;
        }

        public bool CanEdit(EmployeeCreateEditView view)
        {
            return IsUniqueDocumentNumber(view.Id, view.DocumentNumber)
                && IsUniqueInternalCode(view.Id, view.InternalCode)
                && ModelState.IsValid;
        }

        private bool IsUniqueDocumentNumber(int employeeId, string docNumber)
        {
            var alreadyExists = UnitOfWork.Select<Employee>()
                .Where(e => e.DocumentNumber.Equals(docNumber) && e.Id != employeeId)
                .Any();

            if (alreadyExists)
            {
                Alerts.AddError(Validation.For<EmployeeCreateEditView>("NotUniqueDocumentNumber"));
                return false;
            }

            return true;
        }

        private bool IsUniqueInternalCode(int employeeId, string internalCode)
        {
            var alreadyExists = UnitOfWork.Select<Employee>()
                .Where(e => e.InternalCode.Equals(internalCode) && e.Id != employeeId)
                .Any();

            if (alreadyExists)
            {
                Alerts.AddError(Validation.For<EmployeeCreateEditView>("NotUniqueInternalCode"));
                return false;
            }

            return true;
        }

        public bool CanDelete(EmployeeCreateEditView view)
        {
            Alerts.AddError(Validation.For<EmployeeCreateEditView>("DeleteNotAllowed"));
            return false;
        }
    }
}
