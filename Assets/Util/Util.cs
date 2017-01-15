using System;
using System.Collections.Generic;

public static class Util {

    private static Random rng = new Random();  

    public static void Shuffle<T>(this IList<T> list)  
    {  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = rng.Next(n + 1);  
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
    }

    public static string Name(this Patient.Ending ending) {
        switch (ending) {
            case Patient.Ending.Heal: return "被治愈";
            case Patient.Ending.Worsen: return "病情恶化";
            default: return "死亡";
        }
    }

}
