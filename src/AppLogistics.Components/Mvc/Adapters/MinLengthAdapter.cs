using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using AppLogistics.Resources;

namespace AppLogistics.Components.Mvc
{
    public class MinLengthAdapter : AttributeAdapterBase<MinLengthAttribute>
    {
        public MinLengthAdapter(MinLengthAttribute attribute) : base(attribute, null) { }

        public override void AddValidation(ClientModelValidationContext context)
        {
            if (context == null) return;
            context.Attributes["data-val"] = "true";
            context.Attributes["data-val-minlength"] = GetErrorMessage(context);
            context.Attributes["data-val-minlength-min"] = Attribute.Length.ToString();
        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            return Validation.For("MinLength", validationContext.ModelMetadata.PropertyName, Attribute.Length);
        }
    }
}
