using AppLogistics.Objects;
using Humanizer;
using System.Reflection;

namespace AppLogistics.Web.Templates;

public class ModuleModel
{
    public string Model { get; }
    public string Models { get; }
    public string ModelVarName { get; }
    public string ModelShortName { get; }

    public string View { get; }

    public string Service { get; }
    public string IService { get; }
    public string ServiceTests { get; }

    public string Validator { get; }
    public string IValidator { get; }
    public string ValidatorTests { get; }

    public string ControllerTestsNamespace { get; }
    public string ControllerNamespace { get; }
    public string ControllerTests { get; }
    public string Controller { get; }

    public string Area { get; }

    public PropertyInfo[] Properties { get; set; }
    public PropertyInfo[] AllProperties { get; set; }
    public Dictionary<PropertyInfo, string> Relations { get; set; }

    public ModuleModel(string model, string controller, string area)
    {
        //ModelShortName = Regex.Split(model, "(?=[A-Z])").Last();
        ModelShortName = model;
        ModelVarName = model[0].ToString().ToLower() + model.Substring(1);
        Models = model.Pluralize(false);
        Model = model;

        View = $"{Model}View";

        ServiceTests = $"{Model}ServiceTests";
        IService = $"I{Model}Service";
        Service = $"{Model}Service";

        ValidatorTests = $"{Model}ValidatorTests";
        IValidator = $"I{Model}Validator";
        Validator = $"{Model}Validator";

        ControllerTestsNamespace = $"AppLogistics.Controllers.{(!string.IsNullOrWhiteSpace(area) ? $"{area}." : "")}Tests";
        ControllerNamespace = "AppLogistics.Controllers" + (!string.IsNullOrWhiteSpace(area) ? $".{area}" : "");
        ControllerTests = $"{controller}ControllerTests";
        Controller = $"{controller}Controller";

        Area = area;

        Type type = typeof(BaseModel).Assembly.GetType("AppLogistics.Objects." + model) ?? typeof(BaseModel);
        PropertyInfo[] properties = type.GetProperties();

        AllProperties = properties.Where(property => property.PropertyType.Namespace == "System").ToArray();
        Properties = AllProperties.Where(property => property.DeclaringType.Name == model).ToArray();
        Relations = Properties
            .ToDictionary(
                property => property,
                property => properties.SingleOrDefault(relation =>
                    property.Name.EndsWith("Id")
                    && relation.PropertyType.Assembly == type.Assembly
                    && relation.Name == property.Name.Remove(property.Name.Length - 2))?.PropertyType.Name);
    }
}
