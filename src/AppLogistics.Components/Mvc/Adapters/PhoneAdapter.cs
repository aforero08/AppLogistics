using AppLogistics.Resources;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace AppLogistics.Components.Mvc;

public class PhoneAdapter : AttributeAdapterBase<PhoneAttribute>
{
    public PhoneAdapter(PhoneAttribute attribute)
        : base(attribute, null)
    {
    }

    public override void AddValidation(ClientModelValidationContext context)
    {
        context.Attributes["data-val"] = "true";
        context.Attributes["data-val-phone"] = GetErrorMessage(context);
    }

    public override string GetErrorMessage(ModelValidationContextBase validationContext)
    {
        Attribute.ErrorMessage = Validation.For("Phone");

        return GetErrorMessage(validationContext.ModelMetadata);
    }
}
