using System.ComponentModel.DataAnnotations;

namespace AppLogistics.Objects
{
    public class AccountView : BaseView<Account>
    {
        [Required]
        [StringLength(32)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; }

        public bool IsLocked { get; set; }

        public string RoleTitle { get; set; }
    }
}
