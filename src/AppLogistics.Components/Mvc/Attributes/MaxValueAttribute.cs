using AppLogistics.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace AppLogistics.Components.Mvc;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class MaxValueAttribute : ValidationAttribute
{
    public decimal Maximum { get; }

    public MaxValueAttribute(double maximum)
        : base(() => Validation.For("MaxValue"))
    {
        Maximum = Convert.ToDecimal(maximum);
    }

    public override string FormatErrorMessage(string name)
    {
        return string.Format(ErrorMessageString, name, Maximum);
    }

    public override bool IsValid(object value)
    {
        try
        {
            return value == null || Convert.ToDecimal(value) <= Maximum;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
