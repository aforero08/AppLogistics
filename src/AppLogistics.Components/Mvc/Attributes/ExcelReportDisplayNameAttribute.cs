using AppLogistics.Resources;
using System;
using System.ComponentModel;

namespace AppLogistics.Components.Mvc;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class ExcelReportDisplayNameAttribute : DisplayNameAttribute
{
    public ExcelReportDisplayNameAttribute(string reportName, string attribute) : base(GetMessageFromResource(reportName, attribute))
    {
    }

    private static string GetMessageFromResource(string reportName, string attribute)
    {
        return Resource.ForProperty(reportName, attribute);
    }
}
