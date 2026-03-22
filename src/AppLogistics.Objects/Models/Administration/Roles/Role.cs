using AppLogistics.Components.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppLogistics.Objects;

public class Role : BaseModel
{
    [Required]
    [StringLength(128)]
    [Index(IsUnique = true)]
    public string Title { get; set; }

    public virtual List<Account> Accounts { get; set; }
    public virtual List<RolePermission> Permissions { get; set; }
}
