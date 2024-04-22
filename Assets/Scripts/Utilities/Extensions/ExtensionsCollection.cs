using System;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionsCollection
{
    public static T RandomPull<T>(this List<T> self)
    {
        int index = UnityEngine.Random.Range(0, self.Count);
        T obj = self[index]; self.RemoveAt(index);
        return obj;
    }

    public static bool Contains<T>(this Queue<T> self, T other, Func<T, T, bool> comparison)
    {
        foreach (T item in self)
            if (comparison(item, other))
                return true;

        return false;
    }
}
