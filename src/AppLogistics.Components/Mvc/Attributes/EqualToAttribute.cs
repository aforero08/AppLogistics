using AppLogistics.Resources;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AppLogistics.Components.Mvc;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class EqualToAttribute : ValidationAttribute
{
    public string OtherPropertyName { get; }
    public string OtherPropertyDisplayName { get; set; }

    public EqualToAttribute(string otherPropertyName)
        : base(() => Validation.For("EqualTo"))
    {
        OtherPropertyName = otherPropertyName ?? throw new ArgumentNullException(nameof(otherPropertyName));
    }

    public override string FormatErrorMessage(string name)
    {
        return string.Format(ErrorMessageString, name, OtherPropertyDisplayName);
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        PropertyInfo other = validationContext.ObjectType.GetProperty(OtherPropertyName);
        if (other != null && Equals(value, other.GetValue(validationContext.ObjectInstance)))
        {
            return null;
        }

        OtherPropertyDisplayName = Resource.ForProperty(validationContext.ObjectType, OtherPropertyName) ?? OtherPropertyName;

        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
    }
}
