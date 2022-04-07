using UnityEngine;

#if  UNITY_EDITOR
using UnityEditor;
#endif

namespace Utils.TimelineTools
{
    [ExecuteInEditMode]
    public class CameraControl : MonoBehaviour
    {
        public new Camera camera;
        
        private void OnUpdate()
        {
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                Debug.Log($"OnUpdate at {Input.mousePosition.x} {Input.mousePosition.y}");
        }
        private void OnGUI()
        {
#if  UNITY_EDITOR

            if (Event.current.type == EventType.Layout || Event.current.type == EventType.Repaint)
            {
                EditorUtility.SetDirty(this); // this is important, if omitted, "Mouse down" will not be display
            }
            else if (Event.current.type == EventType.MouseDown)
            {
                var evPos = Event.current.mousePosition;
                Debug.Log($"Mouse down {evPos.x} {evPos.x}");
                var pos = Selection.activeTransform.position;
                Selection.activeTransform.position = new Vector3(evPos.x, pos.y, evPos.y);
            }
 #endif       
        }
    }
}