using System;
using System.Collections.Generic;
using System.Linq;

namespace PCL2.Neo.Utils;

public static class RandomUtils
{
    private static readonly Random random = new Random();

    public static T RandomOne<T>(ICollection<T> objects)
    {
        return objects.ElementAt(RandomInteger(0, objects.Count - 1));
    }

    public static int RandomInteger(int min,int max)
    {
        return (int) Math.Floor((max - min + 1) * random.NextDouble()) + min;
    }

    public static IList<T> Shuffle<T>(IList<T> array) { 
        IList<T> result = new List<T>();
        do
        {
            int i = RandomInteger(0, array.Count - 1);
            result.Add(array[i]);
            array.RemoveAt(i);
        }
        while (array.Any());
        return result;
    }
}
