using System.ComponentModel.DataAnnotations;

namespace AppLogistics.Objects;

public class BranchOffice : BaseModel
{
    [Required]
    [StringLength(32)]
    public string Name { get; set; }
}
