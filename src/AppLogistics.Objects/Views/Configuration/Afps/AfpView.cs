using NonFactors.Mvc.Lookup;
using System.ComponentModel.DataAnnotations;

namespace AppLogistics.Objects;

public class AfpView : BaseView<Afp>
{
    [Required]
    [LookupColumn]
    [StringLength(32)]
    public string Name { get; set; }

    [Required]
    [StringLength(16)]
    public string Nit { get; set; }
}
