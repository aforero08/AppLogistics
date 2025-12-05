using NonFactors.Mvc.Lookup;

namespace AppLogistics.Objects
{
    public class ClientView : BaseView<Client>
    {
        [LookupColumn]
        public string Name { get; set; }

        public string Nit { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Contact { get; set; }
    }
}
