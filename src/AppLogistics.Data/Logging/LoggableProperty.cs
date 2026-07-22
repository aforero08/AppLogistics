using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System;

namespace AppLogistics.Data.Logging;

public class LoggableProperty
{
    private object OldValue { get; }
    private object NewValue { get; }
    private string Property { get; }
    public bool IsModified { get; }

    public LoggableProperty(PropertyEntry entry, object newValue)
    {
        NewValue = newValue;
        OldValue = entry.CurrentValue;
        Property = entry.Metadata.Name;
        IsModified = entry.IsModified && !Equals(NewValue, OldValue);
    }

    public override string ToString()
    {
        if (IsModified)
        {
            return Property + ": " + Format(NewValue) + " => " + Format(OldValue);
        }

        return Property + ": " + Format(NewValue);
    }

    private string Format(object value)
    {
        if (value is null)
        {
            return "null";
        }

        if (value is DateTime date)
        {
            return "\"" + date.ToString("yyyy-MM-dd HH:mm:ss") + "\"";
        }

        return JsonConvert.ToString(value);
    }
}
