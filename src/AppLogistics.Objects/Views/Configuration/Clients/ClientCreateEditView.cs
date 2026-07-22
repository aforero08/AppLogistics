using System.ComponentModel.DataAnnotations;

namespace AppLogistics.Objects;

public class ClientCreateEditView : BaseView<Client>
{
    [Required]
    [StringLength(128)]
    public string Name { get; set; }

    [Required]
    [StringLength(16)]
    public string Nit { get; set; }

    [Required]
    [StringLength(64)]
    public string Address { get; set; }

    [StringLength(16, MinimumLength = 7)]
    [Phone]
    public string Phone { get; set; }

    [StringLength(32)]
    public string Contact { get; set; }
}
