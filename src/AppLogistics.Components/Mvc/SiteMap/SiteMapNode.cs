using System.Collections.Generic;

namespace AppLogistics.Components.Mvc;

public class SiteMapNode
{
    public bool IsMenu { get; set; }
    public string IconClass { get; set; }
    public bool IsActive { get; set; }
    public bool HasActiveChildren { get; set; }

    public string Controller { get; set; }
    public string Action { get; set; }
    public string Area { get; set; }

    public SiteMapNode Parent { get; set; }
    public IEnumerable<SiteMapNode> Children { get; set; }
}
