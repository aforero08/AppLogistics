using AppLogistics.Objects;

namespace AppLogistics.Validators;

public interface IServiceReportValidator : IValidator
{
    bool CanQuery(ServiceReportQueryView query);
}
