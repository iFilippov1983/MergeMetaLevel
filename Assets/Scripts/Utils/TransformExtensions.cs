using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utils
{
    public static class TransformExtensions
    {
        public static void Clear(this Transform container)
        {
            container.ForEachReverse(child => child.gameObject.DoDestroy());
        }
        
        public static void ForEachReverse(this Transform parent, Action<Transform> cb)
        {
            for(int i = parent.childCount-1; i >=0; i--)
            {
                var child = parent.GetChild(i);
                cb(child);
            }
        }
        
        public static void PopulateFromPrefab<T>(this Transform container, int needCount, T prefab, ref List<T> items) where T : MonoBehaviour
        {
            if(needCount > items.Count )
                while (items.Count < needCount)
                    items.Add(Object.Instantiate(prefab, container));
            
            items.ForEach(item => item.gameObject.SetActive(false));
        }
    }
}