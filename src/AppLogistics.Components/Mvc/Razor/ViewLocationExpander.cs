using Microsoft.AspNetCore.Mvc.Razor;
using System.Collections.Generic;

namespace AppLogistics.Components.Mvc;

public class ViewLocationExpander : IViewLocationExpander
{
    public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
    {
        if (RazorViewEngine.GetNormalizedRouteValue(context.ActionContext, "area") != null)
        {
            return new[]
            {
                "/Views/{2}/Shared/{0}.cshtml",
                "/Views/{2}/{1}/{0}.cshtml",
                "/Views/Shared/{0}.cshtml"
            };
        }

        return new[]
        {
            "/Views/{1}/{0}.cshtml",
            "/Views/Shared/{0}.cshtml"
        };
    }

    public void PopulateValues(ViewLocationExpanderContext context)
    {
    }
}
