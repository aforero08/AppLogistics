using NonFactors.Mvc.Lookup;
using System.ComponentModel.DataAnnotations;

namespace AppLogistics.Objects;

public class DocumentTypeView : BaseView<DocumentType>
{
    [Required]
    [LookupColumn]
    [StringLength(32)]
    public string Name { get; set; }

    [Required]
    [StringLength(8)]
    public string ShortName { get; set; }
}
