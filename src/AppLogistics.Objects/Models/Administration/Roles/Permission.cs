using System.ComponentModel.DataAnnotations;

namespace AppLogistics.Objects;

public class Permission : BaseModel
{
    [StringLength(64)]
    public string Area { get; set; }

    [Required]
    [StringLength(64)]
    public string Controller { get; set; }

    [Required]
    [StringLength(64)]
    public string Action { get; set; }
}
