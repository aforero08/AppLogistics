using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AppLogistics.Components.Mvc;

public class LettersNumbersAdapter : AttributeAdapterBase<LettersNumbersAttribute>
{
    public LettersNumbersAdapter(LettersNumbersAttribute attribute)
        : base(attribute, null)
    {
    }

    public override void AddValidation(ClientModelValidationContext context)
    {
        context.Attributes["data-val"] = "true";
        context.Attributes["data-val-lettersnumbers"] = GetErrorMessage(context);
    }

    public override string GetErrorMessage(ModelValidationContextBase validationContext)
    {
        return GetErrorMessage(validationContext.ModelMetadata);
    }
}
