using AppLogistics.Resources;

namespace AppLogistics.Components.Extensions.Native;

public static class BooleanExtensions
{
    public static string MapToStringUsingResources(this bool value)
    {
        if (value)
        {
            return Resource.ForString("Yes");
        }

        return Resource.ForString("No");
    }
}
