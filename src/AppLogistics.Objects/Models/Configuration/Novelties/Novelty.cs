using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppLogistics.Objects;

public class Novelty : BaseModel
{
    [Required]
    [StringLength(32)]
    public string Name { get; set; }

    [StringLength(128)]
    public string Description { get; set; }

    public virtual ICollection<ServiceNovelty> ServiceNovelties { get; set; }
}
