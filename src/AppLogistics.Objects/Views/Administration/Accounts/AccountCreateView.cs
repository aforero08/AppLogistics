using AppLogistics.Components.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AppLogistics.Objects;

public class AccountCreateView : BaseView<Account>
{
    [Required]
    [StringLength(32)]
    [LettersNumbers]
    public string Username { get; set; }

    [Required]
    [NotTrimmed]
    [StringLength(32)]
    public string Password { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(256)]
    public string Email { get; set; }

    public int? RoleId { get; set; }
}
