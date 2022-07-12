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
            worksheet.Cells[1, 1].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.ServiceId));
            worksheet.Cells[1, 2].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.CreationDate));
            worksheet.Cells[1, 3].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.CreationTime));
            worksheet.Cells[1, 4].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.EndDate));
            worksheet.Cells[1, 5].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.EndTime));
            worksheet.Cells[1, 6].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.ClientName));
            worksheet.Cells[1, 7].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.CarrierName));
            worksheet.Cells[1, 8].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.ActivityName));
            worksheet.Cells[1, 9].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.VehicleTypeName));
            worksheet.Cells[1, 10].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.VehicleNumber));
            worksheet.Cells[1, 11].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.ProductName));
            worksheet.Cells[1, 12].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.Quantity));
            worksheet.Cells[1, 13].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.RatePrice));
            worksheet.Cells[1, 14].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.RateSplitFare));
            worksheet.Cells[1, 15].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.EmployeesQuantity));
            worksheet.Cells[1, 16].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.ServiceFullPrice));
            worksheet.Cells[1, 17].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.EmployeePercentage));
            worksheet.Cells[1, 18].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.ServiceHoldingPrice));
            worksheet.Cells[1, 19].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.SectorName));
            worksheet.Cells[1, 20].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.CustomsInformation));
            worksheet.Cells[1, 21].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.InternalDocument));
            worksheet.Cells[1, 22].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.ExternalDocument));
            worksheet.Cells[1, 23].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.Location));
            worksheet.Cells[1, 24].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportExcelView.Comments));
            worksheet.Cells[1, 25].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportEmployeeExcelView.EmployeeInternalCode));
            worksheet.Cells[1, 26].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportEmployeeExcelView.EmployeeName));
            worksheet.Cells[1, 27].Value = GetMessageFromResource("ExcelServiceReport", nameof(ServiceReportEmployeeExcelView.EmployeeHoldingPrice));
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
                worksheet.Cells[rowNumber, 1].Value = mappedServices[i].ServiceId;
                worksheet.Cells[rowNumber, 2].Value = mappedServices[i].CreationDate;
                worksheet.Cells[rowNumber, 3].Value = mappedServices[i].CreationTime;
                worksheet.Cells[rowNumber, 4].Value = mappedServices[i].EndDate;
                worksheet.Cells[rowNumber, 5].Value = mappedServices[i].EndTime;
                worksheet.Cells[rowNumber, 6].Value = mappedServices[i].ClientName;
                worksheet.Cells[rowNumber, 7].Value = mappedServices[i].CarrierName;
                worksheet.Cells[rowNumber, 8].Value = mappedServices[i].ActivityName;
                worksheet.Cells[rowNumber, 9].Value = mappedServices[i].VehicleTypeName;
                worksheet.Cells[rowNumber, 10].Value = mappedServices[i].VehicleNumber;
                worksheet.Cells[rowNumber, 11].Value = mappedServices[i].ProductName;
                worksheet.Cells[rowNumber, 12].Value = mappedServices[i].Quantity;
                worksheet.Cells[rowNumber, 13].Value = mappedServices[i].RatePrice;
                worksheet.Cells[rowNumber, 14].Value = mappedServices[i].RateSplitFare.MapToStringUsingResources();
                worksheet.Cells[rowNumber, 15].Value = mappedServices[i].EmployeesQuantity;
                worksheet.Cells[rowNumber, 16].Value = mappedServices[i].ServiceFullPrice;
                worksheet.Cells[rowNumber, 17].Value = mappedServices[i].EmployeePercentage;
                worksheet.Cells[rowNumber, 18].Value = mappedServices[i].ServiceHoldingPrice;
                worksheet.Cells[rowNumber, 19].Value = mappedServices[i].SectorName;
                worksheet.Cells[rowNumber, 20].Value = mappedServices[i].CustomsInformation;
                worksheet.Cells[rowNumber, 21].Value = mappedServices[i].InternalDocument;
                worksheet.Cells[rowNumber, 22].Value = mappedServices[i].ExternalDocument;
                worksheet.Cells[rowNumber, 23].Value = mappedServices[i].Location;
                worksheet.Cells[rowNumber, 24].Value = mappedServices[i].Comments;

                foreach (var employeeData in mappedServices[i].EmployeesInfo)
                {
                    worksheet.Cells[rowNumber, 25].Value = employeeData.EmployeeInternalCode;
                    worksheet.Cells[rowNumber, 26].Value = employeeData.EmployeeName;
                    worksheet.Cells[rowNumber, 27].Value = employeeData.EmployeeHoldingPrice;

                    // Set intercalated fill colors
                    worksheet.Cells[rowNumber, 1, rowNumber, 27].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    if (i % 2 == 0)
                    {
                        worksheet.Cells[rowNumber, 1, rowNumber, 27].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    }
                    else
                    {
                        worksheet.Cells[rowNumber, 1, rowNumber, 27].Style.Fill.BackgroundColor.SetColor(Color.White);
                    }

                    rowNumber++;
                }
            }

            SetTotals(worksheet, rowNumber);
        }

        private void SetTotals(ExcelWorksheet worksheet, int rowNumber)
        {
            // ServiceFullPrice
            worksheet.Cells[rowNumber, 16].Formula = GetFormulaColumnSum(16, 2, rowNumber - 1);
            worksheet.Cells[rowNumber, 16].Style.Font.Bold = true;

            // ServiceHoldingPrice
            worksheet.Cells[rowNumber, 18].Formula = GetFormulaColumnSum(18, 2, rowNumber - 1);
            worksheet.Cells[rowNumber, 18].Style.Font.Bold = true;

            // EmployeeHoldingPrice
            worksheet.Cells[rowNumber, 27].Formula = GetFormulaColumnSum(27, 2, rowNumber - 1);
            worksheet.Cells[rowNumber, 27].Style.Font.Bold = true;
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
            worksheet.Column(2).Style.Numberformat.Format = "yyyy-MM-dd";
            
            // CreationTime
            worksheet.Column(3).Style.Numberformat.Format = "hh:mm";

            // EndDate
            worksheet.Column(4).Style.Numberformat.Format = "yyyy-MM-dd";

            // EndTime
            worksheet.Column(5).Style.Numberformat.Format = "hh:mm";

            // RatePrice 
            worksheet.Column(13).Style.Numberformat.Format = "$ #,##0.00";
            
            // ServiceFullPrice
            worksheet.Column(16).Style.Numberformat.Format = "$ #,##0.00";
            
            // EmployeePercentage
            worksheet.Column(17).Style.Numberformat.Format = "#0\\%";
            
            // ServiceHoldingPrice
            worksheet.Column(18).Style.Numberformat.Format = "$ #,##0.00";
            
            // EmployeeHoldingPrice
            worksheet.Column(27).Style.Numberformat.Format = "$ #,##0.00";

            worksheet.Row(1).Style.Font.Bold = true;
            worksheet.Cells[1, 1, 1, 27].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            worksheet.View.FreezePanes(2, 2);
            worksheet.View.ZoomScale = 90;

            worksheet.Calculate();
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
        }
    }
}
