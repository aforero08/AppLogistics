using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting; // Added for IsDevelopment()
using System.Collections.Concurrent;
using System.IO;

namespace AppLogistics.Components.Mvc;

[HtmlTargetElement("script", Attributes = "action")]
public class AppScriptTagHelper : TagHelper
{
    public override int Order => -2000;
    public string Action { get; set; }

    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; }

    private readonly IWebHostEnvironment _environment;
    private static ConcurrentDictionary<string, string> Scripts { get; }

    static AppScriptTagHelper()
    {
        Scripts = new ConcurrentDictionary<string, string>();
    }

    public AppScriptTagHelper(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        string path = FormPath();

        if (!Scripts.ContainsKey(path))
        {
            Scripts[path] = null;
            if (ScriptsAvailable(path))
            {
                Scripts[path] = new UrlHelper(ViewContext).Content("~/scripts/application/" + path);
            }
        }

        if (Scripts[path] == null)
        {
            output.TagName = null;
        }
        else
        {
            output.Attributes.SetAttribute("src", Scripts[path]);
        }
    }

    private bool ScriptsAvailable(string path)
    {
        return File.Exists(Path.Combine(_environment.WebRootPath, "scripts/application/" + path));
    }

    private string FormPath()
    {
        RouteValueDictionary route = ViewContext.RouteData.Values;
        string extension = _environment.IsDevelopment() ? ".js" : ".min.js";
        return ((route["Area"] == null ? null : route["Area"] + "/") + route["controller"] + "/" + Action + extension).ToLower();
    }
}
