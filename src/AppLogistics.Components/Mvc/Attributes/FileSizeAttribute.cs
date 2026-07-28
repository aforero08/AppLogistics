using AppLogistics.Resources;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AppLogistics.Components.Mvc;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class FileSizeAttribute : ValidationAttribute
{
    public decimal MaximumMB { get; }

    public FileSizeAttribute(double maximumMB)
        : base(() => Validation.For("FileSize"))
    {
        MaximumMB = Convert.ToDecimal(maximumMB);
    }

    public override string FormatErrorMessage(string name)
    {
        return string.Format(ErrorMessageString, name, MaximumMB);
    }

    public override bool IsValid(object value)
    {
        IEnumerable<IFormFile> files = value is IFormFile formFile ? new[] { formFile } : value as IEnumerable<IFormFile>;

        return files == null || files.Sum(file => file?.Length ?? 0) <= MaximumMB * 1024 * 1024;
    }
}
