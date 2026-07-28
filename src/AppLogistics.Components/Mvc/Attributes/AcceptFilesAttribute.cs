using AppLogistics.Resources;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AppLogistics.Components.Mvc;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class AcceptFilesAttribute : ValidationAttribute
{
    public string Extensions { get; }

    public AcceptFilesAttribute(string extensions)
        : base(() => Validation.For("AcceptFiles"))
    {
        Extensions = extensions;
    }

    public override string FormatErrorMessage(string name)
    {
        return string.Format(ErrorMessageString, name, Extensions);
    }

    public override bool IsValid(object value)
    {
        if (value == null)
        {
            return true;
        }

        IEnumerable<IFormFile> files = value is IFormFile formFile ? new[] { formFile } : value as IEnumerable<IFormFile>;

        return files?.All(file => Extensions.Split(',').Any(ext => file.FileName?.EndsWith(ext) == true)) == true;
    }
}
