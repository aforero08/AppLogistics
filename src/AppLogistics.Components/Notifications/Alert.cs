namespace AppLogistics.Components.Notifications;

public class Alert
{
    public string Id { get; set; }
    public int Timeout { get; set; }
    public AlertType Type { get; set; }
    public string Message { get; set; }
}
