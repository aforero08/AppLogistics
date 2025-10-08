using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting; // Added for IsDevelopment()
using System.Collections.Concurrent;
using System.IO;

namespace AppLogistics.Components.Mvc
{
    [HtmlTargetElement("link", Attributes = "action", TagStructure = TagStructure.WithoutEndTag)]
    public class AppStyleTagHelper : TagHelper
    {
        public override int Order => -2000;
        public string Action { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        private readonly IWebHostEnvironment _environment;
        private static ConcurrentDictionary<string, string> Styles { get; }

        static AppStyleTagHelper()
        {
            Styles = new ConcurrentDictionary<string, string>();
        }

        public AppStyleTagHelper(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string path = FormPath();

            if (!Styles.ContainsKey(path))
            {
                Styles[path] = null;
                if (ScriptsAvailable(path))
                {
                    Styles[path] = new UrlHelper(ViewContext).Content("~/content/application/" + path);
                }
            }

            if (Styles[path] == null)
            {
                output.TagName = null;
            }
            else
            {
                output.Attributes.SetAttribute("href", Styles[path]);
            }
        }

        private bool ScriptsAvailable(string path)
        {
            return File.Exists(Path.Combine(_environment.WebRootPath, "content/application/" + path));
        }

        private string FormPath()
        {
            RouteValueDictionary route = ViewContext.RouteData.Values;
            string extension = _environment.IsDevelopment() ? ".css" : ".min.css";
            return ((route["Area"] == null ? null : route["Area"] + "/") + route["controller"] + "/" + Action + extension).ToLower();
        }
    }
}
