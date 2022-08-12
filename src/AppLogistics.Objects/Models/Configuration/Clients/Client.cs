using AppLogistics.Components.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppLogistics.Objects
{
    public class Client : BaseModel
    {
        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        [Required]
        [StringLength(16)]
        [Index(IsUnique = true)]
        public string Nit { get; set; }

        [Required]
        [StringLength(64)]
        public string Address { get; set; }

        [StringLength(16)]
        [Phone]
        public string Phone { get; set; }

        [StringLength(32)]
        public string Contact { get; set; }

        public virtual ICollection<Rate> Rates { get; set; }

        public virtual ICollection<Sector> Sectors { get; set; }
    }
}
