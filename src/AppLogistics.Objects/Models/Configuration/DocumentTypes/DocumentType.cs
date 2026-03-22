using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppLogistics.Objects;

public class DocumentType : BaseModel
{
    [Required]
    [StringLength(32)]
    public string Name { get; set; }

    [Required]
    [StringLength(8)]
    public string ShortName { get; set; }

    public virtual ICollection<Employee> Employees { get; set; }
}
