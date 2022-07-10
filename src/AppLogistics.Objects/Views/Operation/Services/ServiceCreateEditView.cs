using AppLogistics.Components.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AppLogistics.Objects
{
    public class ServiceCreateEditView : BaseView
    {
        [Required]
        public int RateClientId { get; set; }

        [Required]
        public int RateId { get; set; }

        public bool SpecifyVehicleType { get; set; }

        public int? VehicleTypeId { get; set; }

        [MinValue(0.01)]
        public double Quantity { get; set; }

        [Required]
        public int[] SelectedEmployees { get; set; }

        public int? SectorId { get; set; }

        public int? CarrierId { get; set; }

        [StringLength(16)]
        public string VehicleNumber { get; set; }

        [Required]
        [StringLength(32)]
        public string Location { get; set; }

        [StringLength(32)]
        public string CustomsInformation { get; set; }

        public int[] SelectedNovelties { get; set; }

        [StringLength(128)]
        public string Comments { get; set; }
    }
}
