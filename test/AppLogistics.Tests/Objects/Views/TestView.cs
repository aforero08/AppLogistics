using AppLogistics.Objects;
using System.ComponentModel.DataAnnotations;

namespace AppLogistics.Tests;

public class TestView : BaseView
{
    [StringLength(128)]
    public string Title { get; set; }
}
