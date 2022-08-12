using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppLogistics.Objects
{
    public class Sector : BaseModel
    {
        [Required]
        [StringLength(32)]
        public string Name { get; set; }

        public int? ClientId { get; set; }
        public virtual Client Client { get; set; }

        public virtual ICollection<Service> Services { get; set; }
    }
}
