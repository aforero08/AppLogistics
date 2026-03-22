using AppLogistics.Resources;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AppLogistics.Components.Mvc;

public class NumberValidator : IClientModelValidator
{
    public void AddValidation(ClientModelValidationContext context)
    {
        context.Attributes["data-val"] = "true";
        context.Attributes["data-val-number"] = Validation.For("Numeric", context.ModelMetadata.GetDisplayName());
    }
}
