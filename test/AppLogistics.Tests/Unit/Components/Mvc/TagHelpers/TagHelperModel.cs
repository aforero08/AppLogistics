using System.ComponentModel.DataAnnotations;

namespace AppLogistics.Components.Mvc.Tests;

public class TagHelperModel
{
    [Required]
    public string Required { get; set; }

    public string NotRequired { get; set; }
    public long RequiredValue { get; set; }
    public long? NotRequiredNullableValue { get; set; }
}
