using AppLogistics.Resources;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AppLogistics.Components.Mvc;

public class DateValidator : IClientModelValidator
{
    public void AddValidation(ClientModelValidationContext context)
    {
        context.Attributes["data-val"] = "true";
        context.Attributes["data-val-date"] = Validation.For("Date", context.ModelMetadata.GetDisplayName());
    }
}
