using System;
using Api.Ui.LevelComplete;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using Utils.Text;

namespace Components
{
    public class UiLevelComplete : UiBaseView, IPointerDownHandler
    {
        public TextMeshProUGUI XpCount;
        public XpFly XpFly;
        public RectTransform XpIcon;
        public CanvasGroup TapToContinue;
        public ExecuteTween StarFlyTween;
        public CanvasGroup Content;
        public Tweened_TextMeshPro WellDoneText;
        public CoinsMono CoinsMono;
        public TextMeshProUGUI CoinsCount;
        public PlayableDirector PlayableDirector;

        public event Action OnClick;
        public void OnPointerDown(PointerEventData eventData) => OnClick?.Invoke(); 

        // Api
        public UiLevelCompleteNewApi Api;
    }
}