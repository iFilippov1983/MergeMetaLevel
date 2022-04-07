using Data;
using Data.Dynamic;
using UnityEngine;

namespace Components
{
    public class RootView : MonoBehaviour
    {
        public MergeView Merge;
        public UiView Ui;
        public CheatsApi CheatsApi;

        public DynamicData DataPreview;

        public void SetCtx()
        {
            
        }
    }
}