using System;
using System.Collections.Generic;

public static class ExtensionsCollection
{
    public static T RandomPull<T>(this List<T> self)
    {
        int index = UnityEngine.Random.Range(0, self.Count);
        T obj = self[index]; self.RemoveAt(index);
        return obj;
    }
}
