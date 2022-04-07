using UnityEngine;

namespace Utils.Text
{
    public class ScriptableObjectSingleton<T> : ScriptableObject where T : ScriptableObjectSingleton<T> 
    {
        public static T _instance;
        public static T GetInstance(string filePath)
        {
            if (_instance)
                return _instance;
#if UNITY_EDITOR
            _instance  = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(GetEditorAssetPath(filePath));
#else
            _instance = Resources.Load<T>(GetResourcePath(filePath));
#endif
            return _instance;
        }
        
        protected static string GetResourcePath(string resourcesRelativeDir) => $"{resourcesRelativeDir}";
        protected static string GetEditorAssetPath(string resourcesRelativeDir) => $"Assets/Resources/{resourcesRelativeDir}.asset";
    }
}