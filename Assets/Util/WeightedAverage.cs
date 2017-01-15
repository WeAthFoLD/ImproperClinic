using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public interface WeightedItem<T> {
    float weight { get; }
    T val { get; }
}

[System.Serializable]
public static class WeightedAverage {

    [System.Serializable]
    public struct Item {
        public int index;
        public float weight;
    }

    public static List<Item> Normalize(List<Item> items) {
        var normalizedSum = new List<Item>();

        float sum = 0;
        foreach (var item in items) {
            // Hack: 0 weight defaults to 1
            var w = item.weight == 0 ? 1 : item.weight;
            sum += w;
            normalizedSum.Add(new Item { index = item.index, weight = sum });
        }

        return normalizedSum;
    }

    public static int RandomIndex(List<Item> normalizedSum) {
        if (normalizedSum.Count == 0) {
            throw new System.InvalidOperationException("Empty weighted average");
        }
        float max = normalizedSum[normalizedSum.Count - 1].weight;
        float rand = Random.value * max;
        foreach (var item in normalizedSum) {
            if (item.weight >= rand)
                return item.index;
        }
        return normalizedSum[normalizedSum.Count - 1].index;
    }

    public static U RandItem<T, U>(List<T> items, out int selectedIndex){
        var items2 = new List<Item>();
        for(int i = 0; i < items.Count; ++i) {
            var casted = (WeightedItem<U>) items[i];
            items2.Add(new Item { weight = casted.weight, index = i });
        }
        items2 = Normalize(items2);

        var idx = RandomIndex(items2);
        selectedIndex = idx;
        return ((WeightedItem<U>) items[idx]).val;
    }
    
    public static U RandItem<T, U>(List<T> items) {
        int tmp;
        return RandItem<T, U>(items, out tmp);
    }

    public static List<U> SelectN<T, U>(List<T> items, int n) {
        List<U> selected = new List<U>();
        List<T> temp = new List<T>(items);
        for (int i = 0; i < n && temp.Count > 0; ++i) {
            int idx;
            U rand = RandItem<T, U>(temp, out idx);
            selected.Add(rand);
            temp.RemoveAt(idx);
        }

        return selected;
    }

}
