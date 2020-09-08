using System.Collections.Generic;
using System.Linq;

public static class ListExtensions
{
    public static T Random<T>(this List<T> list)
    {
        if (list.Count == 0) return default;

        var index = UnityEngine.Random.Range(0, list.Count());
        return list[index];
    }
}