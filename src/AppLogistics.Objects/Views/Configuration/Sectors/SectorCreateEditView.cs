using System.ComponentModel.DataAnnotations;

namespace AppLogistics.Objects
{
    public class SectorCreateEditView : BaseView
    {
        [Required]
        [StringLength(32)]
        public string Name { get; set; }

        [Required]
        public int ClientId { get; set; }
    }
}
