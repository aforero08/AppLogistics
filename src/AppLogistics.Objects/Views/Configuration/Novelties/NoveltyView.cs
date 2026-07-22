using NonFactors.Mvc.Lookup;
using System.ComponentModel.DataAnnotations;

namespace AppLogistics.Objects;

public class NoveltyView : BaseView<Novelty>
{
    [Required]
    [LookupColumn]
    [StringLength(32)]
    public string Name { get; set; }
    
    [StringLength(128)]
    public string Description { get; set; }
}
