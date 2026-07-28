using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Resources;

namespace AppLogistics.Validators;

public class ServiceReportValidator : BaseValidator, IServiceReportValidator
{
    public ServiceReportValidator(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
    }

    public bool CanQuery(ServiceReportQueryView query)
    {
        return IsValidDateRange(query) && ModelState.IsValid;
    }

    private bool IsValidDateRange(ServiceReportQueryView query)
    {
        if (query.StartDate > query.EndDate)
        {
            Alerts.AddError(Validation.For<ServiceReportQueryView>("InvalidDatesRange"));
            return false;
        }

        return true;
    }
}
