using System;

namespace AppLogistics.Components.Extensions.Native;

public static class DateTimeExtensions
{
    public static DateTime UtcToDefaultTimeZone(this DateTime utcDateTime)
    {
        return utcDateTime.AddHours(-5);
    }
}
