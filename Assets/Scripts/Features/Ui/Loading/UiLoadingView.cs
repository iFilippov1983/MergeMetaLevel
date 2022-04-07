using UnityEngine;

namespace Components
{
    public class UiLoadingView : UiBaseView
    {
        public RectTransform Bg;
        public RectTransform RootCanvasRect;
        // public Canvas Canvas;
        
        public UiLoadingApi Api;
        public int ShowDuration = 800;
    }
}