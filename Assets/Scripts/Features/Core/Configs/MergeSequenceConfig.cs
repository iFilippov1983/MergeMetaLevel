using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Data
{
    public class MergeSequenceConfig : ScriptableObject
    {
        public string Header;
        [InlineEditor, SerializeReference]
        public List<MergeItemConfig> Items;

        public MergeItemConfig Next(MergeItemConfig item)
        {
            var index = Items.IndexOf(item);
            index++;
            if (index >= Items.Count)
                return null;
            return Items[index];
        }

        [Button(ButtonSizes.Large)]
        void Link()
        {
            foreach (var item in Items)
            {
                item.Sequence = this;
#if UNITY_EDITOR
                EditorUtility.SetDirty(item);
#endif
            }
            
// #if UNITY_EDITOR
//             AssetDatabase.SaveAssets();
// #endif
        }
    }
}