using AppLogistics.Resources;
using Microsoft.AspNetCore.Mvc.DataAnnotations.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace AppLogistics.Components.Mvc
{
    public class StringLengthAdapter : StringLengthAttributeAdapter
    {
        public StringLengthAdapter(StringLengthAttribute attribute)
            : base(attribute, null)
        {
        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            Attribute.ErrorMessage = Validation.For(Attribute.MinimumLength == 0 ? "StringLength" : "StringLengthRange");

            return GetErrorMessage(validationContext.ModelMetadata);
        }
    }
}
