using System;

namespace AppLogistics.Components.Mvc;

[AttributeUsage(AttributeTargets.Property)]
public class IndexAttribute : Attribute
{
    public bool IsUnique { get; set; }
}
