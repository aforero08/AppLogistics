using Genny;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppLogistics.Web.Templates;

[GennyAlias("AppLogistics")]
[GennyModuleDescriptor("Default system module template")]
public class Module : GennyModule
{
    [GennyParameter(0, Required = true)]
    public string Model { get; set; }

    [GennyParameter(1, Required = true)]
    public string Controller { get; set; }

    [GennyParameter(2, Required = false)]
    public string Area { get; set; }

    public Module(IServiceProvider services)
        : base(services)
    {
    }

    public override void Run()
    {
        string path = (Area != null ? Area + "/" : "") + Controller;
        Dictionary<string, GennyScaffoldingResult> results = new Dictionary<string, GennyScaffoldingResult>();

        results.Add($"../AppLogistics.Resources/Resources/Views/{path}/{Model}View.json", Scaffold("Resources/View"));
        results.Add($"../AppLogistics.Resources/Resources/Views/{path}/{Model}View.es.json", Scaffold("Resources/ViewEs"));

        results.Add($"../AppLogistics.Controllers/{path}/{Controller}Controller.cs", Scaffold("Controllers/Controller"));
        results.Add($"../../test/AppLogistics.Tests/Unit/Controllers/{path}/{Controller}ControllerTests.cs", Scaffold("Tests/ControllerTests"));

        results.Add($"../AppLogistics.Objects/Models/{path}/{Model}.cs", Scaffold("Objects/Model"));
        results.Add($"../AppLogistics.Objects/Views/{path}/{Model}View.cs", Scaffold("Objects/View"));

        results.Add($"../AppLogistics.Services/{path}/{Model}Service.cs", Scaffold("Services/Service"));
        results.Add($"../AppLogistics.Services/{path}/I{Model}Service.cs", Scaffold("Services/IService"));
        results.Add($"../../test/AppLogistics.Tests/Unit/Services/{path}/{Model}ServiceTests.cs", Scaffold("Tests/ServiceTests"));

        results.Add($"../AppLogistics.Validators/{path}/{Model}Validator.cs", Scaffold("Validators/Validator"));
        results.Add($"../AppLogistics.Validators/{path}/I{Model}Validator.cs", Scaffold("Validators/IValidator"));
        results.Add($"../../test/AppLogistics.Tests/Unit/Validators/{path}/{Model}ValidatorTests.cs", Scaffold("Tests/ValidatorTests"));

        results.Add($"../AppLogistics.Web/Views/{path}/Index.cshtml", Scaffold("Web/Index"));
        results.Add($"../AppLogistics.Web/Views/{path}/Create.cshtml", Scaffold("Web/Create"));
        results.Add($"../AppLogistics.Web/Views/{path}/Details.cshtml", Scaffold("Web/Details"));
        results.Add($"../AppLogistics.Web/Views/{path}/Edit.cshtml", Scaffold("Web/Edit"));
        results.Add($"../AppLogistics.Web/Views/{path}/Delete.cshtml", Scaffold("Web/Delete"));

        if (results.Any(result => result.Value.Errors.Any()))
        {
            Dictionary<string, GennyScaffoldingResult> errors = new Dictionary<string, GennyScaffoldingResult>(results.Where(x => x.Value.Errors.Any()));

            Write(errors);

            Logger.WriteLine("");
            Logger.WriteLine("Scaffolding failed! Rolling back...", ConsoleColor.Red);
        }
        else
        {
            Logger.WriteLine("");

            TryWrite(results);

            Logger.WriteLine("");
            Logger.WriteLine("Scaffolded successfully!", ConsoleColor.Green);
        }
    }

    public override void ShowHelp()
    {
        Logger.WriteLine("Parameters:");
        Logger.WriteLine("    1 - Scaffolded model.");
        Logger.WriteLine("    2 - Scaffolded controller.");
        Logger.WriteLine("    3 - Scaffolded area (optional).");
    }

    private GennyScaffoldingResult Scaffold(string path)
    {
        return Scaffolder.Scaffold("Templates/Module/" + path, new ModuleModel(Model, Controller, Area));
    }
}
