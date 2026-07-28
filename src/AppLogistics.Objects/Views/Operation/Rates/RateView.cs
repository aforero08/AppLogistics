using NonFactors.Mvc.Lookup;

namespace AppLogistics.Objects;

public class RateView : BaseView<Rate>
{
    [LookupColumn]
    public string Name { get; set; }

    public int ClientId { get; set; }

    public string ClientName { get; set; }

    [LookupColumn]
    public string ActivityName { get; set; }

    [LookupColumn]
    public string VehicleTypeName { get; set; }

    [LookupColumn]
    public string ProductName { get; set; }

    public decimal Price { get; set; }

    public float EmployeePercentage { get; set; }

    public bool SplitFare { get; set; }
}
