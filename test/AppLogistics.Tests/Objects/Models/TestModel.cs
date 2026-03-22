using AppLogistics.Objects;
using System.ComponentModel.DataAnnotations;

namespace AppLogistics.Tests;

public class TestModel : BaseModel
{
    [StringLength(128)]
    public string Title { get; set; }
}
