using System;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Components
{
    public class UiMergeToolView : UiBaseView, IPointerClickHandler
    {
        public Button CloseBtn;
        public TextMeshProUGUI CaptionText;
        public TextMeshProUGUI InfoText;
        public Action OnClick;

        // Api
        public UiMergeToolViewApi Api;
        
        public void OnPointerClick(PointerEventData eventData) 
            => OnClick?.Invoke();
    }
}