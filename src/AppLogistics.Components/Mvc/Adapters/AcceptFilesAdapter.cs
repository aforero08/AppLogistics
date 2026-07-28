using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AppLogistics.Components.Mvc;

public class AcceptFilesAdapter : AttributeAdapterBase<AcceptFilesAttribute>
{
    public AcceptFilesAdapter(AcceptFilesAttribute attribute)
        : base(attribute, null)
    {
    }

    public override void AddValidation(ClientModelValidationContext context)
    {
        context.Attributes["data-val"] = "true";
        context.Attributes["data-val-acceptfiles"] = GetErrorMessage(context);
        context.Attributes["data-val-acceptfiles-extensions"] = Attribute.Extensions;
    }

    public override string GetErrorMessage(ModelValidationContextBase validationContext)
    {
        return GetErrorMessage(validationContext.ModelMetadata);
    }
}
