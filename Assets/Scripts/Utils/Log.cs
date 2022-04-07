using UnityEngine;

public static class Log
{
    public static void NullWarning<T>(T obj, string objInfo)
    {
        if(obj == null)
            Debug.LogWarning($"{objInfo} is NULL");
    }
}