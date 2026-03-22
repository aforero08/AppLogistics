using AppLogistics.Components.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppLogistics.Objects;

public class Rate : BaseModel
{
    [Required]
    [StringLength(32)]
    [Index(IsUnique = true)]
    public string Name { get; set; }

    public int ClientId { get; set; }
    public virtual Client Client { get; set; }

    public int ActivityId { get; set; }
    public virtual Activity Activity { get; set; }

    public int? VehicleTypeId { get; set; }
    public virtual VehicleType VehicleType { get; set; }

    public int? ProductId { get; set; }
    public virtual Product Product { get; set; }

    public decimal Price { get; set; }

    public float EmployeePercentage { get; set; }

    public bool SplitFare { get; set; }

    public virtual ICollection<Service> Services { get; set; }
}
