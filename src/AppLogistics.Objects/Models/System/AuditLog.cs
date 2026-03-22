using System.ComponentModel.DataAnnotations;

namespace AppLogistics.Objects;

public class AuditLog : BaseModel
{
    public int? AccountId { get; set; }

    [Required]
    [StringLength(16)]
    public string Action { get; set; }

    [Required]
    [StringLength(64)]
    public string EntityName { get; set; }

    public int EntityId { get; set; }

    [Required]
    public string Changes { get; set; }
}
