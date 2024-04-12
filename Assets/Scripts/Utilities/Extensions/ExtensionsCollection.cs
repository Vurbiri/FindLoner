using System.Collections.Generic;
using UnityEngine;

public static class ExtensionsCollection
{
    public static T RandomPop<T>(this List<T> self)
    {
        int index = Random.Range(0, self.Count);
        T obj = self[index]; self.RemoveAt(index);
        return obj;
    }
}
