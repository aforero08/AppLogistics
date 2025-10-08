using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using AppLogistics.Resources;

namespace AppLogistics.Components.Mvc
{
    public class RangeAdapter : AttributeAdapterBase<RangeAttribute>
    {
        public RangeAdapter(RangeAttribute attribute) : base(attribute, null) { }

        public override void AddValidation(ClientModelValidationContext context)
        {
            if (context == null) return;
            context.Attributes["data-val"] = "true";
            context.Attributes["data-val-range"] = GetErrorMessage(context);
            context.Attributes["data-val-range-min"] = Attribute.Minimum.ToString();
            context.Attributes["data-val-range-max"] = Attribute.Maximum.ToString();
        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            return Validation.For("Range", validationContext.ModelMetadata.PropertyName, Attribute.Minimum, Attribute.Maximum);
        }
    }
}
