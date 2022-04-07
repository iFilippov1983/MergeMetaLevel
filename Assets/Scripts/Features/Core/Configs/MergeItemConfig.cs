using Sirenix.OdinInspector;
using UnityEngine;

namespace Data
{
    public class MergeItemConfig : SerializedScriptableObject, IEditorMergeItem
    {
        [PreviewField, HideLabel]
        public Sprite Sprite;
        
        [ReadOnly] 
        public MergeSequenceConfig Sequence;
        
        public int count;
        
        public bool isGenerator;
        [ShowIf("@isGenerator")]
        public MergeItemConfig GeneratedLife;
        [ShowIf("@isGenerator")]
        public GenerateComp Generate;
        
        public bool isLife;
        // [ShowIf("@isLife")]
        
        public bool isResource;
        [ShowIf("@isResource")]
        public string resourceName;
        
        
        public Sprite GetSprite() => Sprite;

        public string GetName() => name;

        public MergeItemConfig Next() => Sequence.Next(this);

    }
}