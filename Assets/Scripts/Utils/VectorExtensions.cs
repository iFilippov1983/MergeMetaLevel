using UnityEngine;

namespace Utils
{
    public static class VectorExtensions
    {
        public static Vector3 ToVector3(this Vector2Int v2) 
            => new Vector3(v2.x, v2.y);
        
        public static Vector3 ToVector3(this V2 v2) 
            => new Vector3(v2.x, v2.y);
        
        public static Vector3 ToVector3(this V3 v2) 
            => new Vector3(v2.x, v2.y);
    }
}