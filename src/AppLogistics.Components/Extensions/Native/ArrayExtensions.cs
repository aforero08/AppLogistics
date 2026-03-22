namespace AppLogistics.Components.Extensions.Native;

public static class ArrayExtensions
{
    public static bool IsNullOrEmpty<T>(this T[] array)
    {
        if (array == null)
        {
            return true;
        }

        return array.Length == 0;
    }
}
