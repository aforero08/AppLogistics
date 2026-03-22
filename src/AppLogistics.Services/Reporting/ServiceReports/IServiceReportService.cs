using AppLogistics.Objects;
using System.Linq;

namespace AppLogistics.Services;

public interface IServiceReportService : IService
{
    IQueryable<ServiceReportView> FilterByQuery(ServiceReportQueryView query);
    ServiceReportView GetDetail(int id);
    byte[] GetExcelReport(ServiceReportQueryView query);
}
