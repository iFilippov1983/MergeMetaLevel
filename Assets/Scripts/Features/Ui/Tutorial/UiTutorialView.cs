using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Components
{
    public class UiTutorialView : UiBaseView
    {
        public TextMeshProUGUI Text;
        public RectTransform TextContainer;
        public RectTransform HandContainer;
        public RectTransform ArrowContainer;
        public RectTransform Bg;
        public RectTransform BgMini;
        public ClickReceiverView ClickReceiver;
        public HorizontalLayoutGroup TextLayoutGroup;

        public UiTutorialApi Api;
        

        // [Button]
        // void Editor_Init()
        // {
        //     Api ??= new UiTutorialApi();
        //     Api.SetView(this);
        // }

        // [Button]
        // void Editor_Redraw() 
        //     => Api?.Redraw();
        
        // public void OnPointerClick(PointerEventData eventData)
        // {
        //     Debug.Log( "Clicked!" );
        //     _onClick?.Invoke();
        // }
        
    }
}