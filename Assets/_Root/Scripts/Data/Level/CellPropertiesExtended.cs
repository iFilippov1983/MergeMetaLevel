using UnityEditor;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "GameData/Cells/CellPropertiesExtended", fileName = "Cell_n_event")]
    public class CellPropertiesExtended : ScriptableObject
    {
        [MenuItem("Tools/MyTool/Do It in C#")]
        static void DoIt()
        {
            EditorUtility.DisplayDialog("MyTool", "Do It in C# !", "OK", "");
        }
    }
}