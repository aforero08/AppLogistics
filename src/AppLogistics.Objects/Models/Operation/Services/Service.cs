using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppLogistics.Objects
{
    public class Service : BaseModel
    {
        public int RateId { get; set; }
        public virtual Rate Rate { get; set; }

        public double Quantity { get; set; }

        public int? SectorId { get; set; }
        public virtual Sector Sector { get; set; }

        public int? CarrierId { get; set; }
        public virtual Carrier Carrier { get; set; }

        public int? VehicleTypeId { get; set; }
        public virtual VehicleType VehicleType { get; set; }

        [StringLength(16)]
        public string VehicleNumber { get; set; }

        [Required]
        [StringLength(32)]
        public string Location { get; set; }

        [StringLength(32)]
        public string CustomsInformation { get; set; }

        public decimal FullPrice { get; set; }

        public decimal HoldingPrice { get; set; }

        [StringLength(128)]
        public string Comments { get; set; }

        public DateTime? EndDate { get; set; }

        [StringLength(32)]
        public string InternalDocument { get; set; }

        [StringLength(32)]
        public string ExternalDocument { get; set; }

        public virtual ICollection<Holding> Holdings { get; set; }

        public virtual ICollection<ServiceNovelty> ServiceNovelties { get; set; }
    }
}
