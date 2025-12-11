using System;
using System.Collections.Generic;

namespace CustomExtensions
{
    public static class LogicExtensions
    {
        public static bool IsSameElement<T>(this Dictionary<T,int> map, List<T> a, List<T> b)
        {
            if (map.Count > 0) map.Clear();

            foreach (var item in a)
            {
                map[item] = map.TryGetValue(item, out var value) ? value + 1 : 1;
            }

            foreach (var item in b)
            {
                if (map.TryGetValue(item, out var value) == false) return false;

                if (value > 1) map[item]--;
                else map.Remove(item);
            }

            return map.Count == 0;
        }

        public static T GetItemByFlags<T>(this IList<T> list, int flagValue)
        {
            for (int i = 0; i < sizeof(int) * 8; i++)
            {
                if ((flagValue & (1 << i)) != 0 && list.Count > i) return list[i];
            }
            
            return default;
        }
    }
}
