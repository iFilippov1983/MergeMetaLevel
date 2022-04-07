using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Data
{
    public class EditorConfig : SerializedScriptableObject
    {
        public static IEditorItem Selected;
        
        public LevelConfig Level;
        [InlineEditor()]
        public MergeBgConfig BgConfig;
        public List<EditorSequence> EditorSequences;
        
        [ShowInInspector, PreviewField, Sirenix.OdinInspector.ReadOnly , PropertyOrder(-1), HideLabel] 
        public Sprite Preview => Selected != null ? Selected.GetSprite() : null;
        

        [Serializable]
        public class EditorBg
        {
            [ShowInInspector, PreviewField, Sirenix.OdinInspector.ReadOnly] public Sprite Preview => Item != null ? Item.Sprite : null;
            public MergeBgConfig Item;

            [Button(ButtonSizes.Large)]
            void Select()
            {
                EditorConfig.Selected = this.Item;
            }
        }

        [Serializable]
        public class EditorItem
        {
            
            [ShowInInspector, PreviewField] public Sprite Preview => Item != null ? Item.Sprite : null;
            public MergeItemConfig Item;

            [Button(ButtonSizes.Large)]
            void Select()
            {
                EditorConfig.Selected = this.Item;
            }
        }

        [Serializable]
        public class EditorSequence
        {
            public MergeSequenceConfig Sequence;
            [ TableList] public List<EditorItem> EditorItems;
            [Button]
            void Fill()
            {
                EditorItems.Clear();
                foreach (var itemConfig in Sequence.Items)
                    EditorItems.Add(new EditorItem(){Item = itemConfig}); 
            }
        }

       
    }
}