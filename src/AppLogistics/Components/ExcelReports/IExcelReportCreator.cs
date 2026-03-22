using AppLogistics.Objects;
using System.Collections.Generic;

namespace AppLogistics.Components.ExcelReports;

public interface IExcelReportCreator
{
    byte[] CreateServiceReport(IList<ServiceReportExcelView> mappedServices);
}
