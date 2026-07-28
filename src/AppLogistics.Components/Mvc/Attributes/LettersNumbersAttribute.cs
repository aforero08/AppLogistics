using AppLogistics.Resources;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AppLogistics.Components.Mvc;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class LettersNumbersAttribute : ValidationAttribute
{
    public LettersNumbersAttribute()
        : base(() => Validation.For("LettersNumbers"))
    {
    }

    public override bool IsValid(object value)
    {
        return value == null || Regex.IsMatch(value.ToString(), "^[a-zA-Z0-9]+$");
    }
}
