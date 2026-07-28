using AppLogistics.Components.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AppLogistics.Objects;

public class ProfileDeleteView : BaseView
{
    [Required]
    [NotTrimmed]
    [StringLength(32)]
    public string Password { get; set; }
}
