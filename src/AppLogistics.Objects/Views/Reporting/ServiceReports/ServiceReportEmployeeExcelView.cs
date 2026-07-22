using AppLogistics.Components.Mvc;

namespace AppLogistics.Objects;

public class ServiceReportEmployeeExcelView
{
    [ExcelReportDisplayName("ExcelServiceReport", nameof(EmployeeInternalCode))]
    public string EmployeeInternalCode { get; set; }

    [ExcelReportDisplayName("ExcelServiceReport", nameof(EmployeeName))]
    public string EmployeeName { get; set; }

    [ExcelReportDisplayName("ExcelServiceReport", nameof(EmployeeHoldingPrice))]
    public decimal EmployeeHoldingPrice { get; set; }
}
