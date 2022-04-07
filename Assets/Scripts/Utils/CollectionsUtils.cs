using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public static class CollectionsUtils
    {
        public static void For<T, TT>(List<T> list1, List<TT> list2, Action<T, TT> cb)
        {
            var maxCount = Math.Min(list1.Count, list2.Count);
            for (int i = 0; i < maxCount; i++)
                cb(list1[i], list2[i]);
        }

        public static T Random<T>(this List<T> list)
        {
            if(list.Count ==  0)
                return default(T);
            var rnd = UnityEngine.Random.Range(0, list.Count);
            return list[rnd];
        }
        
        public static KeyValuePair<TKey, TVal> Random<TKey, TVal>(this Dictionary<TKey, TVal> map)
        {
            var randomElement = map.ElementAt(UnityEngine.Random.Range(0, map.Count));
            // map.Remove(randomElement.Key);
            return randomElement;
        }
        
        public static T GetByIndexOrLast<T>(this List<T> list, int index)
        {
            if(list.Count == 0)
                return default(T);

            if (index < list.Count)
                return list[index];

            var last = list[list.Count - 1];
            return last;
        }
        
        
        public static TVal SaveGet<TKey, TVal>(this Dictionary<TKey, TVal> dictionary, TKey key)
        {
            dictionary.TryGetValue(key, out var res);
            return res;
        }
        
        public static T SaveGet<T>(this List<T> list, int index)
        {
            if (index >= list.Count)
                return default;
            return list[index];
        }
    
        public static T[] Clone<T>(this T[] array)
        {
            T[] res = new T[array.Length]; 
            Array.Copy(array, res, array.Length);
            return res;
        }
        public static List<T> Clone<T>(this List<T> array) 
            => new List<T>(array);
        
        public static List<T> DoRemove<T>(this List<T> array, T item)
        {
            array.Remove(item);
            return array;
        }
        public static List<T> DoAdd<T>(this List<T> array, T item)
        {
            array.Add(item);
            return array;
        }
    }
}