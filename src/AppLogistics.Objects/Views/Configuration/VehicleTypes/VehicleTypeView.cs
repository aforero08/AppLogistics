using NonFactors.Mvc.Lookup;
using System.ComponentModel.DataAnnotations;

namespace AppLogistics.Objects;

public class VehicleTypeView : BaseView<VehicleType>
{
    [Required]
    [LookupColumn]
    [StringLength(32)]
    public string Name { get; set; }
}
