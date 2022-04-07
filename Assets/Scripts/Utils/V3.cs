using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Utils
{
    [Serializable]
    public class V3
    {
        public float x;
        public float y;
        public float z;

        public V3(Vector3 v)
        {
            x = v.x;
            y = v.y;
            z = v.z;
        }
        
        public V3(float xx, float yy, float zz)
        {
            x = xx;
            y = yy;
            z = zz;
        }

        public Vector3 ToVector3() 
            => new Vector3(x, y, z);
        
        // [HorizontalGroup()]
        // [HorizontalGroup()]
        // [HorizontalGroup()]
    }
}