using AppLogistics.Components.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace AppLogistics.Objects;

public class Account : BaseModel
{
    [Required]
    [StringLength(32)]
    [Index(IsUnique = true)]
    public string Username { get; set; }

    [Required]
    [StringLength(64)]
    public string Passhash { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(256)]
    [Index(IsUnique = true)]
    public string Email { get; set; }

    public bool IsLocked { get; set; }

    [StringLength(36)]
    public string RecoveryToken { get; set; }

    public DateTime? RecoveryTokenExpirationDate { get; set; }

    public int? RoleId { get; set; }
    public virtual Role Role { get; set; }
}
