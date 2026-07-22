using System.Collections.Generic;
using System.Linq;

namespace AppLogistics.Components.Extensions.Native;

public static class EnumerableExtensions
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable) => enumerable == null || !enumerable.Any();
}
