using System.Globalization;

namespace AppLogistics.Components.Mvc;

public class Language
{
    public string Name { get; set; }
    public string Abbreviation { get; set; }
    public CultureInfo Culture { get; set; }
}
