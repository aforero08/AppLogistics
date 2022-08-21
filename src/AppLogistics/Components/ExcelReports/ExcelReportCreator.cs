using AppLogistics.Components.Extensions.Native;
using AppLogistics.Objects;
using AppLogistics.Resources;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace AppLogistics.Components.ExcelReports
{
    public class ExcelReportCreator : IExcelReportCreator
    {
        public byte[] CreateServiceReport(IList<ServiceReportExcelView> mappedServices)
        {
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Report");

                WriteTittles(worksheet);

                WriteInfo(worksheet, mappedServices);

                FormatSheet(worksheet);

                return excelPackage.GetAsByteArray();
            }
        }

        private void WriteTittles(ExcelWorksheet worksheet)
        {
            worksheet.Cells[1, ColumnNumberConstants.ServiceId].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.ServiceId));
            worksheet.Cells[1, ColumnNumberConstants.CreationDate].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.CreationDate));
            worksheet.Cells[1, ColumnNumberConstants.CreationTime].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.CreationTime));
            worksheet.Cells[1, ColumnNumberConstants.EndDate].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.EndDate));
            worksheet.Cells[1, ColumnNumberConstants.EndTime].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.EndTime));
            worksheet.Cells[1, ColumnNumberConstants.ClientName].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.ClientName));
            worksheet.Cells[1, ColumnNumberConstants.CarrierName].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.CarrierName));
            worksheet.Cells[1, ColumnNumberConstants.ActivityName].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.ActivityName));
            worksheet.Cells[1, ColumnNumberConstants.VehicleTypeName].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.VehicleTypeName));
            worksheet.Cells[1, ColumnNumberConstants.VehicleNumber].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.VehicleNumber));
            worksheet.Cells[1, ColumnNumberConstants.ProductName].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.ProductName));
            worksheet.Cells[1, ColumnNumberConstants.Quantity].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.Quantity));
            worksheet.Cells[1, ColumnNumberConstants.RatePrice].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.RatePrice));
            worksheet.Cells[1, ColumnNumberConstants.RateSplitFare].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.RateSplitFare));
            worksheet.Cells[1, ColumnNumberConstants.EmployeesQuantity].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.EmployeesQuantity));
            worksheet.Cells[1, ColumnNumberConstants.ServiceFullPrice].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.ServiceFullPrice));
            worksheet.Cells[1, ColumnNumberConstants.EmployeePercentage].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.EmployeePercentage));
            worksheet.Cells[1, ColumnNumberConstants.ServiceHoldingPrice].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.ServiceHoldingPrice));
            worksheet.Cells[1, ColumnNumberConstants.SectorName].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.SectorName));
            worksheet.Cells[1, ColumnNumberConstants.CustomsInformation].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.CustomsInformation));
            worksheet.Cells[1, ColumnNumberConstants.InternalDocument].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.InternalDocument));
            worksheet.Cells[1, ColumnNumberConstants.ExternalDocument].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.ExternalDocument));
            worksheet.Cells[1, ColumnNumberConstants.Location].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.Location));
            worksheet.Cells[1, ColumnNumberConstants.Novelties].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.Novelties));
            worksheet.Cells[1, ColumnNumberConstants.Comments].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.Comments));
            worksheet.Cells[1, ColumnNumberConstants.EmployeeInternalCode].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportEmployeeExcelView.EmployeeInternalCode));
            worksheet.Cells[1, ColumnNumberConstants.EmployeeName].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportEmployeeExcelView.EmployeeName));
            worksheet.Cells[1, ColumnNumberConstants.EmployeeHoldingPrice].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportEmployeeExcelView.EmployeeHoldingPrice));
        }

        private string GetMessageFromResource(string reportName, string attribute)
        {
            return Resource.ForProperty(reportName, attribute);
        }

        private void WriteInfo(ExcelWorksheet worksheet, IList<ServiceReportExcelView> mappedServices)
        {
            var rowNumber = 2;

            for (int i = 0; i < mappedServices.Count; i++)
            {
                worksheet.Cells[rowNumber, ColumnNumberConstants.ServiceId].Value = mappedServices[i].ServiceId;
                worksheet.Cells[rowNumber, ColumnNumberConstants.CreationDate].Value = mappedServices[i].CreationDate;
                worksheet.Cells[rowNumber, ColumnNumberConstants.CreationTime].Value = mappedServices[i].CreationTime;
                worksheet.Cells[rowNumber, ColumnNumberConstants.EndDate].Value = mappedServices[i].EndDate;
                worksheet.Cells[rowNumber, ColumnNumberConstants.EndTime].Value = mappedServices[i].EndTime;
                worksheet.Cells[rowNumber, ColumnNumberConstants.ClientName].Value = mappedServices[i].ClientName;
                worksheet.Cells[rowNumber, ColumnNumberConstants.CarrierName].Value = mappedServices[i].CarrierName;
                worksheet.Cells[rowNumber, ColumnNumberConstants.ActivityName].Value = mappedServices[i].ActivityName;
                worksheet.Cells[rowNumber, ColumnNumberConstants.VehicleTypeName].Value = mappedServices[i].VehicleTypeName;
                worksheet.Cells[rowNumber, ColumnNumberConstants.VehicleNumber].Value = mappedServices[i].VehicleNumber;
                worksheet.Cells[rowNumber, ColumnNumberConstants.ProductName].Value = mappedServices[i].ProductName;
                worksheet.Cells[rowNumber, ColumnNumberConstants.Quantity].Value = mappedServices[i].Quantity;
                worksheet.Cells[rowNumber, ColumnNumberConstants.RatePrice].Value = mappedServices[i].RatePrice;
                worksheet.Cells[rowNumber, ColumnNumberConstants.RateSplitFare].Value = mappedServices[i].RateSplitFare.MapToStringUsingResources();
                worksheet.Cells[rowNumber, ColumnNumberConstants.EmployeesQuantity].Value = mappedServices[i].EmployeesQuantity;
                worksheet.Cells[rowNumber, ColumnNumberConstants.ServiceFullPrice].Value = mappedServices[i].ServiceFullPrice;
                worksheet.Cells[rowNumber, ColumnNumberConstants.EmployeePercentage].Value = mappedServices[i].EmployeePercentage;
                worksheet.Cells[rowNumber, ColumnNumberConstants.ServiceHoldingPrice].Value = mappedServices[i].ServiceHoldingPrice;
                worksheet.Cells[rowNumber, ColumnNumberConstants.SectorName].Value = mappedServices[i].SectorName;
                worksheet.Cells[rowNumber, ColumnNumberConstants.CustomsInformation].Value = mappedServices[i].CustomsInformation;
                worksheet.Cells[rowNumber, ColumnNumberConstants.InternalDocument].Value = mappedServices[i].InternalDocument;
                worksheet.Cells[rowNumber, ColumnNumberConstants.ExternalDocument].Value = mappedServices[i].ExternalDocument;
                worksheet.Cells[rowNumber, ColumnNumberConstants.Location].Value = mappedServices[i].Location;
                worksheet.Cells[rowNumber, ColumnNumberConstants.Novelties].Value = mappedServices[i].Novelties;
                worksheet.Cells[rowNumber, ColumnNumberConstants.Comments].Value = mappedServices[i].Comments;

                foreach (var employeeData in mappedServices[i].EmployeesInfo)
                {
                    worksheet.Cells[rowNumber, ColumnNumberConstants.EmployeeInternalCode].Value = employeeData.EmployeeInternalCode;
                    worksheet.Cells[rowNumber, ColumnNumberConstants.EmployeeName].Value = employeeData.EmployeeName;
                    worksheet.Cells[rowNumber, ColumnNumberConstants.EmployeeHoldingPrice].Value = employeeData.EmployeeHoldingPrice;

                    // Set intercalated fill colors
                    worksheet.Cells[rowNumber, 1, rowNumber, ColumnNumberConstants.LastColumn].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    if (i % 2 == 0)
                    {
                        worksheet.Cells[rowNumber, 1, rowNumber, ColumnNumberConstants.LastColumn].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    }
                    else
                    {
                        worksheet.Cells[rowNumber, 1, rowNumber, ColumnNumberConstants.LastColumn].Style.Fill.BackgroundColor.SetColor(Color.White);
                    }

                    rowNumber++;
                }
            }

            SetTotals(worksheet, rowNumber);
        }

        private void SetTotals(ExcelWorksheet worksheet, int rowNumber)
        {
            // ServiceFullPrice
            worksheet.Cells[rowNumber, ColumnNumberConstants.ServiceFullPrice].Formula = GetFormulaColumnSum(ColumnNumberConstants.ServiceFullPrice, 2, rowNumber - 1);
            worksheet.Cells[rowNumber, ColumnNumberConstants.ServiceFullPrice].Style.Font.Bold = true;

            // ServiceHoldingPrice
            worksheet.Cells[rowNumber, ColumnNumberConstants.ServiceHoldingPrice].Formula = GetFormulaColumnSum(ColumnNumberConstants.ServiceHoldingPrice, 2, rowNumber - 1);
            worksheet.Cells[rowNumber, ColumnNumberConstants.ServiceHoldingPrice].Style.Font.Bold = true;

            // EmployeeHoldingPrice
            worksheet.Cells[rowNumber, ColumnNumberConstants.EmployeeHoldingPrice].Formula = GetFormulaColumnSum(ColumnNumberConstants.EmployeeHoldingPrice, 2, rowNumber - 1);
            worksheet.Cells[rowNumber, ColumnNumberConstants.EmployeeHoldingPrice].Style.Font.Bold = true;
        }

        private string GetFormulaColumnSum(int columnNumber, int initialRow, int lastRow)
        {
            return $"=SUM({GetExcelColumnName(columnNumber)}{initialRow}:{GetExcelColumnName(columnNumber)}{lastRow})";
        }

        private string GetExcelColumnName(int columnNumber)
        {
            const int lettersInAlphabet = 26;
            const int letterAInUnicode = 65;

            int dividend = columnNumber;
            string columnName = string.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % lettersInAlphabet;
                columnName = Convert.ToChar(letterAInUnicode + modulo).ToString() + columnName;
                dividend = (dividend - modulo) / lettersInAlphabet;
            }

            return columnName;
        }

        private void FormatSheet(ExcelWorksheet worksheet)
        {
            // CreationDate
            worksheet.Column(ColumnNumberConstants.CreationDate).Style.Numberformat.Format = "yyyy-MM-dd";

            // CreationTime
            worksheet.Column(ColumnNumberConstants.CreationTime).Style.Numberformat.Format = "hh:mm";

            // EndDate
            worksheet.Column(ColumnNumberConstants.EndDate).Style.Numberformat.Format = "yyyy-MM-dd";

            // EndTime
            worksheet.Column(ColumnNumberConstants.EndTime).Style.Numberformat.Format = "hh:mm";

            // RatePrice 
            worksheet.Column(ColumnNumberConstants.RatePrice).Style.Numberformat.Format = "$ #,##0.00";

            // ServiceFullPrice
            worksheet.Column(ColumnNumberConstants.ServiceFullPrice).Style.Numberformat.Format = "$ #,##0.00";

            // EmployeePercentage
            worksheet.Column(ColumnNumberConstants.EmployeePercentage).Style.Numberformat.Format = "#0\\%";

            // ServiceHoldingPrice
            worksheet.Column(ColumnNumberConstants.ServiceHoldingPrice).Style.Numberformat.Format = "$ #,##0.00";

            // EmployeeHoldingPrice
            worksheet.Column(ColumnNumberConstants.EmployeeHoldingPrice).Style.Numberformat.Format = "$ #,##0.00";

            worksheet.Row(1).Style.Font.Bold = true;
            worksheet.Cells[1, 1, 1, ColumnNumberConstants.LastColumn].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            worksheet.View.FreezePanes(2, 2);
            worksheet.View.ZoomScale = 90;

            worksheet.Calculate();
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
        }
    }
}
