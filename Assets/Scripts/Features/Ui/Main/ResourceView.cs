using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.DoTween;

namespace Components.Main
{
    public class ResourceView : MonoBehaviour
    {
        public Button Button;
        public TextMeshProUGUI CountText;
        public Image Image;
        private int _count;
        private int _countOffset;
        private DG.Tweening.Tween _tw;

        public event Action OnClick;

        public void SetCount(int value)
        {
            _count = value - _countOffset;
            CountText.text = _count.ToString();
        }
        
        public int TweenCount
        {
            set
            {
                _count = value - _countOffset;
                _tw?.Kill();
                _tw = CountText.DoInt(_count, 1.2f);
            }
        }

        public int Count
        {
            set
            {
                _count = value - _countOffset;
                CountText.text = _count.ToString();
            }
        }

        public void SetCountOffset(int countOffset)
        {
            _countOffset = countOffset;
        }

        public void TweenCountOffset()
        {
            int start = _count;
            var end = _count + _countOffset;
            
            _countOffset = 0;
            _count = end;
            var iconTr = (Image.transform as RectTransform);
            iconTr.DOAnchorPosY(iconTr.anchoredPosition.y + 40, 0.5f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutFlash);
            start.TweenTo(end, SetValue, 1.2f).SetDelay(0.3f);
        }

        private void SetValue(int value)
        {
            CountText.text = value.ToString();
        }

        private void Awake()
        {
            if(Button != null)
                Button.onClick.AddListener(HandleClick);
        }

        private void HandleClick()
        {
            OnClick?.Invoke();
        }
        
    }
}