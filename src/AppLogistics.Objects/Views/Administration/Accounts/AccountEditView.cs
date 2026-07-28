using AppLogistics.Components.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AppLogistics.Objects;

public class AccountEditView : BaseView<Account>
{
    [Required]
    [StringLength(32)]
    [LettersNumbers]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(256)]
    public string Email { get; set; }

    public bool IsLocked { get; set; }

    public int? RoleId { get; set; }
}
