using Sirenix.OdinInspector;
using UnityEngine;

namespace Components
{
    public class UiWellDoneView : UiBaseView
    {
        public RectTransform RootCanvasRect;
        public RectTransform Container;
        public GameObject Confety;
        public CanvasGroup ContainerGroup;

        // Api
        public UiWellDoneApi Api;

        public float k = 0.5f;
        
        [Button]
        async void TestSHow()
        {
            Api.SetCtx(this, null);
            await Api.Show(k);
        }
        
        [Button]
        async void TestHide()
        {
            Api.SetCtx(this, null);
            await Api.Hide(k);
        }
    }
}