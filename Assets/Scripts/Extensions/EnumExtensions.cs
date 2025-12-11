using System;

namespace CustomExtensions
{
    public static class EnumExtensions
    {
        public static bool HasFlag<T>(this T value, T comparer) where T : Enum
        {
            var a = Convert.ToInt64(value);
            var b = Convert.ToInt64(comparer);
            return (a & b) == b;
        }
    }
}
