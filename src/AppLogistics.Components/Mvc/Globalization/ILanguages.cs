namespace AppLogistics.Components.Mvc;

public interface ILanguages
{
    Language Default { get; }
    Language[] Supported { get; }
    Language Current { get; set; }

    Language this[string abbreviation] { get; }
}
