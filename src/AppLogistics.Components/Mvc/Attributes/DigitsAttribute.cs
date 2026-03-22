using AppLogistics.Resources;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AppLogistics.Components.Mvc;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class DigitsAttribute : ValidationAttribute
{
    public DigitsAttribute()
        : base(() => Validation.For("Digits"))
    {
    }

    public override bool IsValid(object value)
    {
        return value == null || Regex.IsMatch(value.ToString(), "^[0-9]+$");
    }
}
