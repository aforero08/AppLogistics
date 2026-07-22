namespace AppLogistics.Objects;

public class ServiceReportView : BaseView<Service>
{
    public string RateClientName { get; set; }

    public string RateActivityName { get; set; }

    public string RateVehicleTypeName { get; set; }

    public string RateProductName { get; set; }

    public double Quantity { get; set; }

    public int[] SelectedEmployees { get; set; }

    public string SectorName { get; set; }

    public string CarrierName { get; set; }

    public string VehicleNumber { get; set; }

    public string Location { get; set; }

    public string CustomsInformation { get; set; }

    public string InternalDocument { get; set; }

    public string ExternalDocument { get; set; }

    public decimal FullPrice { get; set; }

    public decimal HoldingPrice { get; set; }

    public int[] SelectedNovelties { get; set; }

    public string Comments { get; set; }
}
