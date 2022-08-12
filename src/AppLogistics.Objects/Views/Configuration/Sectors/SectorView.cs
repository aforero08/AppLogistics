using NonFactors.Mvc.Lookup;

namespace AppLogistics.Objects
{
    public class SectorView : BaseView
    {
        [LookupColumn]
        public string Name { get; set; }

        public string ClientName { get; set; }
    }
}
