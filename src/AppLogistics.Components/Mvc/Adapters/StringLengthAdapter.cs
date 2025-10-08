using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using AppLogistics.Resources;

namespace AppLogistics.Components.Mvc
{
    public class StringLengthAdapter : AttributeAdapterBase<StringLengthAttribute>
    {
        public StringLengthAdapter(StringLengthAttribute attribute) : base(attribute, null) { }

        public override void AddValidation(ClientModelValidationContext context)
        {
            if (context == null) return;
            context.Attributes["data-val"] = "true";
            context.Attributes["data-val-length"] = GetErrorMessage(context);
            context.Attributes["data-val-length-max"] = Attribute.MaximumLength.ToString();
            if (Attribute.MinimumLength > 0)
            {
                context.Attributes["data-val-length-min"] = Attribute.MinimumLength.ToString();
            }
        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            string key = Attribute.MinimumLength == 0 ? "StringLength" : "StringLengthRange";
            return Attribute.MinimumLength == 0
                ? Validation.For(key, validationContext.ModelMetadata.PropertyName, Attribute.MaximumLength)
                : Validation.For(key, validationContext.ModelMetadata.PropertyName, Attribute.MaximumLength, Attribute.MinimumLength);
        }
    }
}
