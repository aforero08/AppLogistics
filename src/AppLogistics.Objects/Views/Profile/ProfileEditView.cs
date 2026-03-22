using AppLogistics.Components.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AppLogistics.Objects;

public class ProfileEditView : BaseView<Account>
{
    [Required]
    [StringLength(32)]
    public string Username { get; set; }

    [Required]
    [NotTrimmed]
    [StringLength(32)]
    public string Password { get; set; }

    [NotTrimmed]
    [StringLength(32)]
    public string NewPassword { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(256)]
    public string Email { get; set; }
}
