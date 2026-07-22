using AppLogistics.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace AppLogistics.Components.Mvc;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class MinValueAttribute : ValidationAttribute
{
    public decimal Minimum { get; }

    public MinValueAttribute(double minimum)
        : base(() => Validation.For("MinValue"))
    {
        Minimum = Convert.ToDecimal(minimum);
    }

    public override string FormatErrorMessage(string name)
    {
        return string.Format(ErrorMessageString, name, Minimum);
    }

    public override bool IsValid(object value)
    {
        try
        {
            return value == null || Convert.ToDecimal(value) >= Minimum;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
