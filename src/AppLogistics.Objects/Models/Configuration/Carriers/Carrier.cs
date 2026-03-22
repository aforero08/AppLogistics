using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppLogistics.Objects;

public class Carrier : BaseModel
{
    [Required]
    [StringLength(32)]
    public string Name { get; set; }

    [Required]
    [StringLength(16)]
    public string Nit { get; set; }

    public virtual ICollection<Service> Services { get; set; }
}
