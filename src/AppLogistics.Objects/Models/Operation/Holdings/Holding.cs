namespace AppLogistics.Objects;

public class Holding : BaseModel
{
    public int ServiceId { get; set; }
    public virtual Service Service { get; set; }

    public int EmployeeId { get; set; }
    public virtual Employee Employee { get; set; }

    public decimal Price { get; set; }
}
