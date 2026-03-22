using AppLogistics.Components.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AppLogistics.Objects;

public class AccountResetView : BaseView<Account>
{
    [Required]
    [StringLength(36)]
    public string Token { get; set; }

    [Required]
    [NotTrimmed]
    [StringLength(32)]
    public string NewPassword { get; set; }
}
