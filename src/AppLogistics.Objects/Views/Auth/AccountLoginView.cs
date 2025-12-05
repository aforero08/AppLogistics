using AppLogistics.Components.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AppLogistics.Objects
{
    public class AccountLoginView : BaseView<Account>
    {
        [Required]
        [StringLength(32)]
        public string Username { get; set; }

        [Required]
        [NotTrimmed]
        [StringLength(32)]
        public string Password { get; set; }
    }
}
