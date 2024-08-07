using Godot;
using System.Linq;
using GDCollection = Godot.Collections;
using NetCollection = System.Collections.Generic;

namespace MenuControl.addons.MenuControl.Helpers;

public static class VariantConverter
{
    public static NetCollection.Dictionary<TKey, TValue> ConvertToNetDictionary<[MustBeVariant] TKey, [MustBeVariant] TValue>(this Variant variant)
    {
        var temp = (GDCollection.Dictionary<TKey, TValue>)variant;
        var dict = temp.ToDictionary(k => k.Key, k => k.Value);
        return dict;
    }

    public static GDCollection.Dictionary<TKey, TValue> ConvertToGodotDictionary<[MustBeVariant] TKey, [MustBeVariant] TValue>(this NetCollection.Dictionary<TKey, TValue> dictionary)
    {
        GDCollection.Dictionary<TKey, TValue> godotDict = new();
        foreach (NetCollection.KeyValuePair<TKey, TValue> item in dictionary)
        {
            godotDict.Add(item.Key, item.Value);
        }
        return godotDict;
    }
}
