using System.ComponentModel.DataAnnotations;

namespace AppLogistics.Objects;

public class AccountRecoveryView : BaseView<Account>
{
    [Required]
    [EmailAddress]
    [StringLength(256)]
    public string Email { get; set; }
}
