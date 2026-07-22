using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace AppLogistics.Components.Mvc;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter)]
public class BindExcludeIdAttribute : Attribute, IPropertyFilterProvider
{
    public Func<ModelMetadata, bool> PropertyFilter { get; }

    public BindExcludeIdAttribute()
    {
        PropertyFilter = (metadata) => metadata.PropertyName != "Id";
    }
}
