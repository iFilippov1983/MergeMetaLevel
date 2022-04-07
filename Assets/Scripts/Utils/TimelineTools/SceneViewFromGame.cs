#if UNITY_EDITOR 
using UnityEditor;
#endif

using Sirenix.OdinInspector;
using UnityEngine;

namespace Utils.TimelineTools
{
    public class SceneViewFromGame : ScriptableObject
    {
        // public class SceneViewCameraTest : ScriptableObject
        // {
            // [MenuItem("Test/Move Scene View Camera")]

        
#if UNITY_EDITOR 
            [Button]
            public static void MoveSceneViewCamera()
            {
                Vector3 position = SceneView.lastActiveSceneView.pivot;
                position.z -= 10.0f;
                 
                SceneView.lastActiveSceneView.pivot = Selection.activeTransform.position;
                SceneView.lastActiveSceneView.Repaint();
            }
#endif
        
    }
}