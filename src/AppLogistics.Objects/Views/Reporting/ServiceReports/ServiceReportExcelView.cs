using AppLogistics.Components.Mvc;
using System;
using System.Collections.Generic;

namespace AppLogistics.Objects
{
    public class ServiceReportExcelView
    {
        [ExcelReportDisplayName("ExcelServiceReport", nameof(ServiceId))]
        public int ServiceId { get; set; }

        [ExcelReportDisplayName("ExcelServiceReport", nameof(CreationDate))]
        public DateTime CreationDate { get; set; }

        [ExcelReportDisplayName("ExcelServiceReport", nameof(CreationTime))]
        public DateTime CreationTime { get; set; }

        [ExcelReportDisplayName("ExcelServiceReport", nameof(EndDate))]
        public DateTime? EndDate { get; set; }

        [ExcelReportDisplayName("ExcelServiceReport", nameof(EndTime))]
        public DateTime? EndTime { get; set; }

        [ExcelReportDisplayName("ExcelServiceReport", nameof(ClientName))]
        public string ClientName { get; set; }

        [ExcelReportDisplayName("ExcelServiceReport", nameof(CarrierName))]
        public string CarrierName { get; set; }

        [ExcelReportDisplayName("ExcelServiceReport", nameof(VehicleNumber))]
        public string VehicleNumber { get; set; }

        [ExcelReportDisplayName("ExcelServiceReport", nameof(ActivityName))]
        public string ActivityName { get; set; }

        [ExcelReportDisplayName("ExcelServiceReport", nameof(VehicleTypeName))]
        public string VehicleTypeName { get; set; }

        [ExcelReportDisplayName("ExcelServiceReport", nameof(ProductName))]
        public string ProductName { get; set; }

        [ExcelReportDisplayName("ExcelServiceReport", nameof(Quantity))]
        public double Quantity { get; set; }

        [ExcelReportDisplayName("ExcelServiceReport", nameof(RatePrice))]
        public decimal RatePrice { get; set; }

        [ExcelReportDisplayName("ExcelServiceReport", nameof(RateSplitFare))]
        public bool RateSplitFare { get; set; }

        [ExcelReportDisplayName("ExcelServiceReport", nameof(EmployeesQuantity))]
        public int EmployeesQuantity { get; set; }

        [ExcelReportDisplayName("ExcelServiceReport", nameof(ServiceFullPrice))]
        public decimal ServiceFullPrice { get; set; }

        [ExcelReportDisplayName("ExcelServiceReport", nameof(EmployeePercentage))]
        public float EmployeePercentage { get; set; }

        [ExcelReportDisplayName("ExcelServiceReport", nameof(ServiceHoldingPrice))]
        public decimal ServiceHoldingPrice { get; set; }

        [ExcelReportDisplayName("ExcelServiceReport", nameof(SectorName))]
        public string SectorName { get; set; }

        [ExcelReportDisplayName("ExcelServiceReport", nameof(CustomsInformation))]
        public string CustomsInformation { get; set; }

        [ExcelReportDisplayName("ExcelServiceReport", nameof(InternalDocument))]
        public string InternalDocument { get; set; }

        [ExcelReportDisplayName("ExcelServiceReport", nameof(ExternalDocument))]
        public string ExternalDocument { get; set; }

        [ExcelReportDisplayName("ExcelServiceReport", nameof(Location))]
        public string Location { get; set; }

        [ExcelReportDisplayName("ExcelServiceReport", nameof(Comments))]
        public string Comments { get; set; }

        [ExcelReportDisplayName("ExcelServiceReport", nameof(EmployeesInfo))]
        public IEnumerable<ServiceReportEmployeeExcelView> EmployeesInfo { get; set; }
    }
}
