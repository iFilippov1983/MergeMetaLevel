using UnityEngine;

namespace Utils
{
    public static class GameObjectExtensions
    {
        public static void DoDestroy(this Object go)
        {
            if (go == null)
                return;
            if(Application.isPlaying)
                Object.Destroy(go);
            else
                Object.DestroyImmediate(go);
        }
        
        public static void DoDelayedDestroy(this GameObject obj, int delay)
            => Async.DelayedCall(delay, () => obj.DoDestroy());
    }
}