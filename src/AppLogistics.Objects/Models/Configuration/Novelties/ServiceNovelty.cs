using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppLogistics.Objects;

public class ServiceNovelty : BaseModel
{
    [Required]
    public int ServiceId { get; set; }

    public virtual Service Service { get; set; }


    [Required]
    public int NoveltyId { get; set; }

    public virtual Novelty Novelty { get; set; }
}
