using System;
using CharTween;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Utils.Text
{
    public class Tweened_TextMeshPro : MonoBehaviour
    {
        public TextMeshProUGUI Text;
        public Curved_TextMeshPro _curved;
        private CanvasGroup _canvasGroup;
        private CharTweener _tweener;

        private Sequence _sequence;

        public void HideImmediate()
        {
            InitAll();
            _canvasGroup.alpha = 0;
        }

        private void InitAll()
        {
            if(_tweener != null)
                return;
            
            _canvasGroup = GetOrAddComponent<CanvasGroup>();
            _tweener = GetOrAddComponent<CharTweener>();
            _canvasGroup.alpha = 0;
        }

        private T GetOrAddComponent<T>() where T : Component
        {
            if (TryGetComponent<T>(out T res))
                return res;
            res = gameObject.AddComponent<T>();
            return res;
        }

        [Button]
        public void ShowText()
        {
            ShowTextTween(0, Text.textInfo.characterCount);
        }

        [Button]
        public void HideText()
        {
            HideTextTween(0, Text.textInfo.characterCount);
        }


        private void ShowTextTween(int start, int end)
        {
            InitAll();
            Reset();
            _canvasGroup.alpha = 0;
            _canvasGroup.DOFade(1, 0.2f);
            for (var i = start; i <= end; ++i)
            {
                var timeOffset = Mathf.Lerp(0, 0.7f, (i - start) / (float)(end - start + 1));
                _tweener.DOFade(i, 0, 0.5f).From().SetDelay(i == 0 ? 0 : timeOffset);
                _tweener.DOScale(i, 0, 0.5f).From().SetEase(Ease.OutBack, 5).SetDelay(timeOffset);
                
            }
        }

        private void HideTextTween(int start, int end)
        {
            for (var i = start; i <= end; ++i)
            {
                var timeOffset = Mathf.Lerp(0, 1, (i - start) / (float)(end - start + 1));
//                _tweener.GetProxyTransform(i).DOPause();
                _tweener.DOFade(i, 0, 0.5f).SetDelay(timeOffset);
                _tweener.DOScale(i, 0, 0.5f).SetEase(Ease.InBack, 5).SetDelay(timeOffset);
            }
            
            Async.DelayedCall(200, () => _canvasGroup.alpha = 0);
        }

        private void Reset()
        {
            _canvasGroup.alpha = 0;
            _tweener.Clear();
            _tweener.Text = Text;
            _curved.ChangeTextVertices();
            _tweener.Initialize();
        }

        private static VertexGradient VertexGradient()
        {
            var gradient = new VertexGradient(
                new Color(0xFE, 0xE1, 0x69, 0xFF),
                new Color(0xFE, 0xE1, 0x69, 0xFF),
                new Color(0xFE, 0xC1, 0x22, 0xFF),
                new Color(0xFE, 0xC1, 0x22, 0xFF));
            return gradient;
        }

        private Sequence  TweenAll(int start, int end)
        {
            var sequence = DOTween.Sequence();

            for (var i = start; i <= end; ++i)
            {
                var timeOffset = Mathf.Lerp(0, 1, (i - start) / (float)(end - start + 1));
                var charSequence = DOTween.Sequence();
                
                var circleTween = _tweener.DOLocalCircle(i, 0.35f, 0.5f)
                    .SetEase(Ease.Linear)
                    .SetLoops(8, LoopType.Restart);
                circleTween.fullPosition = timeOffset;
                charSequence.Append(circleTween);
                
                charSequence
//                    .Join(_tweener.DOLocalMoveY(i, 0.5f, 0.5f).SetEase(Ease.InOutCubic))
                    .Join(_tweener.DOFade(i, 0, 0.5f).From())
                    .Join(_tweener.DOScale(i, 0, 0.5f).From().SetEase(Ease.OutBack, 5));
//                    .Append(_tweener.DOLocalMoveY(i, 0, 0.1f).SetEase(Ease.OutBounce));
                
                charSequence
                    .Append(_tweener.DOLocalMoveY(i, 0.5f, 0.5f).SetEase(Ease.InBounce))
                    .Join(_tweener.DOFade(i, 0, 0.5f))
                    .Join(_tweener.DOScale(i, 0, 0.5f).SetEase(Ease.InBack, 5))
                    .Append(_tweener.DOLocalMoveY(i, 0.5f, 0.5f).SetEase(Ease.InOutCubic));
                
                sequence.Insert(timeOffset, charSequence);
            }

            return sequence;
        } 
        
    }
}