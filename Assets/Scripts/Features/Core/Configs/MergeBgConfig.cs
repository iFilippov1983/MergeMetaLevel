using Sirenix.OdinInspector;
using UnityEngine;

namespace Data
{
    public class MergeBgConfig : SerializedScriptableObject, IEditorBg
    {
        [PreviewField( ObjectFieldAlignment.Center), HideLabel, HorizontalGroup("one", 120)]
        public Sprite Sprite;
        public Sprite GetSprite() => Sprite;
        
        [Button(ButtonSizes.Large), HorizontalGroup("one/right")]
        void Select()
        {
            EditorConfig.Selected = this;
        }
    }
}