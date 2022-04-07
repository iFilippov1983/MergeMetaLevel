using System;
using System.Collections.Generic;

namespace Utils
{
    public static class MathUtils
    {
        public static int IncLoop(this int value, int max)
        {
            ++value;
            if (value > max)
                value = 0;
            return value;
        }
        public static float Remap (this float value, float from1, float to1, float from2, float to2) {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }   
        public static int ToMs (this float value) => (int) (value * 1000);

        // public static float Map (this float x, float x1, float x2, float y1,  float y2)
        // {
        //     var m = (y2 - y1) / (x2 - x1);
        //     var c = y1 - m * x1; // point of interest: c is also equal to y2 - m * x2, though float math might lead to slightly different results.
        //
        //     return m * x + c;
        // }
        
        public static void Shuffle<T>(this List<T> list)  
        {  
            Random rnd = new Random();
            int n = list.Count;  
            while (n > 1) 
            {  
                n--;  
                int k = rnd.Next(n + 1);  
                T value = list[k];  
                list[k] = list[n];  
                list[n] = value;  
            }  
        }
        
        public static string GUID_Short() => Convert.ToBase64String(Guid.NewGuid().ToByteArray());
    }
}