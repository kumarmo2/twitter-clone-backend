using System.Linq;
using System.Collections.Generic;

namespace CommonLibs.Extensions
{
    public static class IEnumerableExtensions
    {
        public static bool IsEmpty<T>(this IEnumerable<T> enumerable) => enumerable == null || !enumerable.Any();
        public static bool IsNotEmpty<T>(this IEnumerable<T> enumerable) => !enumerable.IsEmpty();
    }
}