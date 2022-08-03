using System;

namespace AppLogistics.Objects
{
    public class ServiceReportQueryView
    {
        public int? ServiceId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string[] ClientIds { get; set; }

        public int[] ActivityIds { get; set; }

        public int[] EmployeeIds { get; set; }

        public int[] VehicleTypeIds { get; set; }

        public int[] ProductIds { get; set; }

        public int[] CarrierIds { get; set; }

        public int[] SectorIds { get; set; }

        public int[] NoveltyIds { get; set; }

        public string VehicleNumber { get; set; }

        public string Location { get; set; }

        public string CustomsInformation { get; set; }

        public string InternalDocument { get; set; }

        public string ExternalDocument { get; set; }

        public string Comments { get; set; }
    }
}
