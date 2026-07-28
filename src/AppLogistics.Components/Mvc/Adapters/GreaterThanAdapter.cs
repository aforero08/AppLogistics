using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Globalization;

namespace AppLogistics.Components.Mvc;

public class GreaterThanAdapter : AttributeAdapterBase<GreaterThanAttribute>
{
    public GreaterThanAdapter(GreaterThanAttribute attribute)
        : base(attribute, null)
    {
    }

    public override void AddValidation(ClientModelValidationContext context)
    {
        context.Attributes["data-val"] = "true";
        context.Attributes["data-val-greater"] = GetErrorMessage(context);
        context.Attributes["data-val-greater-min"] = Attribute.Minimum.ToString(CultureInfo.InvariantCulture);
    }

    public override string GetErrorMessage(ModelValidationContextBase validationContext)
    {
        return GetErrorMessage(validationContext.ModelMetadata);
    }
}
