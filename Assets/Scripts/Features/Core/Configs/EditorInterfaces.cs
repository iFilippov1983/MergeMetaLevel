using UnityEngine;

namespace Data
{
    public interface IEditorItem
    {
        Sprite GetSprite();
    }
    
    public interface IEditorMergeItem : IEditorItem{}
    public interface IEditorBg : IEditorItem{}

}