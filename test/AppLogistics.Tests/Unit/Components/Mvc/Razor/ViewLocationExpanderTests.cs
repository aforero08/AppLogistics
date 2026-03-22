using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using Xunit;

namespace AppLogistics.Components.Mvc.Tests;

public class ViewLocationExpanderTests
{
    #region ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<String> locations)

    [Fact]
    public void ExpandViewLocations_Area_ReturnsAreaLocations()
    {
        ActionContext context = new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor());
        ViewLocationExpanderContext expander = new ViewLocationExpanderContext(context, "Index", null, null, null, true);
        context.RouteData.Values["area"] = "Test";

        IEnumerable<string> expected = new[] { "/Views/{2}/Shared/{0}.cshtml", "/Views/{2}/{1}/{0}.cshtml", "/Views/Shared/{0}.cshtml" };
        IEnumerable<string> actual = new ViewLocationExpander().ExpandViewLocations(expander, null);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ExpandViewLocations_ReturnsViewLocations()
    {
        ActionContext context = new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor());
        ViewLocationExpanderContext expander = new ViewLocationExpanderContext(context, "Index", null, null, null, true);

        IEnumerable<string> expected = new[] { "/Views/{1}/{0}.cshtml", "/Views/Shared/{0}.cshtml" };
        IEnumerable<string> actual = new ViewLocationExpander().ExpandViewLocations(expander, null);

        Assert.Equal(expected, actual);
    }

    #endregion ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<String> locations)

    #region PopulateValues(ViewLocationExpanderContext context)

    [Fact]
    public void PopulateValues_DoesNothing()
    {
        new ViewLocationExpander().PopulateValues(null);
    }

    #endregion PopulateValues(ViewLocationExpanderContext context)
}
