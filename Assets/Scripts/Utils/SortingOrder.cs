using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Utils
{
    public class SortingOrder : MonoBehaviour
    {
        public bool customTarget;
        [ShowIf("@customTarget")]
        public GameObject target;
        public bool setToChilds;
        
        [Indent()]
        [ValueDropdown("SortingValues")]
        public int sortingLayerId = 0;
        public int sortingOrder;

        private void OnEnable()
        {
            var tar = customTarget ? target : this.gameObject;
            
            SetSorting(tar.GetComponent<Renderer>());
            
            if (setToChilds)
                tar.GetComponentsInChildren<Renderer>().ForEach(SetSorting);
        }

        private void SetSorting(Renderer renderer)
        {
            if(!renderer)
                return;
            
            renderer.sortingLayerID = sortingLayerId;
            renderer.sortingOrder = sortingOrder;
        }

#if UNITY_EDITOR
        private List<ValueDropdownItem<int>> SortingValues()
        {
            var names = GetSortingLayerNames();
            var ids = GetSortingLayerIds();
            var res = new List<ValueDropdownItem<int>>();

            for (int i = 0; i < names.Length; i++) 
                res.Add(new ValueDropdownItem<int>(names[i], ids[i]));

            return res;
        }

        public static string[] GetSortingLayerNames()
        {
            System.Type internalEditorUtilityType = typeof(UnityEditorInternal.InternalEditorUtility);
            PropertyInfo sortingLayersProperty =
                internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
            return (string[])sortingLayersProperty.GetValue(null, new object[0]);
        }
        public static int[] GetSortingLayerIds()
        {
            System.Type internalEditorUtilityType = typeof(UnityEditorInternal.InternalEditorUtility);
            PropertyInfo sortingLayersProperty =
                internalEditorUtilityType.GetProperty("sortingLayerUniqueIDs", BindingFlags.Static | BindingFlags.NonPublic);
            return (int[])sortingLayersProperty.GetValue(null, new object[0]);
        }
#endif
        
    }
}