using UnityEngine;
using Utils.Text;

namespace Components
{
    public class UiLoadingView : UiBaseView
    {
        public RectTransform Bg;
        public RectTransform RootCanvasRect;
        public Tweened_TextMeshPro Header1;
        public Tweened_TextMeshPro Header2;
        // public Canvas Canvas;
        
        public UiLoadingApi Api;
        public int ShowDuration = 1200;
        public int Header1Delay = 100;
        public int Header2Delay = 500;
        public int Header1HideDelay = 800;
        public int Header2HideDelay = 1000;
    }
}