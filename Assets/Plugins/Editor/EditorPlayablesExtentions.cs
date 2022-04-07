using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

public static class EditorPlayablesExtentions
{
    public static void DisplayBindings(this PlayableDirector director)
    {
        var obj = new SerializedObject(director);
        var bindings = obj.FindProperty("m_SceneBindings");
        for (int i = 0; i < bindings.arraySize; i++)
        {
            var binding = bindings.GetArrayElementAtIndex(i);
            var trackProp = binding.FindPropertyRelative("key");
            var sceneObjProp = binding.FindPropertyRelative("value");
            var track = trackProp.objectReferenceValue;
            var sceneObj = sceneObjProp.objectReferenceValue;

            Debug.LogFormat("Binding {0} {1}", track != null ? track.name : "Null", sceneObj != null ? sceneObj.name : "Null");
        }
    }
}