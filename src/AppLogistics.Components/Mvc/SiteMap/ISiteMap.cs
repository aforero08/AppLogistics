using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace AppLogistics.Components.Mvc;

public interface ISiteMap
{
    IEnumerable<SiteMapNode> For(ViewContext context);

    IEnumerable<SiteMapNode> BreadcrumbFor(ViewContext context);
}
