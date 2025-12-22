using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using AppLogistics.Resources;

namespace AppLogistics.Components.Mvc;

public class RequiredAdapter : AttributeAdapterBase<RequiredAttribute>
{
    public RequiredAdapter(RequiredAttribute attribute) : base(attribute, null) { }

    public override void AddValidation(ClientModelValidationContext context)
    {
        if (context == null) return;
        context.Attributes["data-val"] = "true";
        context.Attributes["data-val-required"] = GetErrorMessage(context);
    }

    public override string GetErrorMessage(ModelValidationContextBase validationContext)
    {
        return Validation.For("Required", validationContext.ModelMetadata.GetDisplayName());
    }
}
