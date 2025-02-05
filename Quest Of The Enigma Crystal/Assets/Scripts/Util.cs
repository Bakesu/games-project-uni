using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static void RenameKey<TKey, TValue>(this IDictionary<TKey, TValue> dic,
                                          TKey fromKey, TKey toKey)
    {
        TValue value = dic[fromKey];
        dic.Remove(fromKey);
        dic[toKey] = value;
    }

    public static K GetKey<K, V>(this IDictionary<K, V> instance, V value)
    {
        foreach (var entry in instance)
        {
            if (!entry.Value.Equals(value))
            {
                continue;
            }
            return entry.Key;
        }
        return default(K);
    }

}

