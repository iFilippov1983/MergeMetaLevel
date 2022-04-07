using System;
using System.Linq;
using Components;
using DG.Tweening;
using Tutorial.UI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Api.Ui
{
    public static class TutorialExtensions
    {
        public static UiTutorialItem FindTarget(string guid, bool includeInactive)
        {
            if (string.IsNullOrEmpty(guid))
                return null;
            
            var targets = Object.FindObjectsOfType<UiTutorialItem>(includeInactive).ToList();
            if (targets.Count == 0)
                Debug.LogWarning($"UiTutorialItem.Count == 0");

            var target = targets.Find(x => x.Guid == guid);
            if (target == null)
                Debug.LogWarning($"Can not find target for tutorial with Guid: {guid}");

            return target;
        }
        
        public static DG.Tweening.Tween TweenFadeIn(this UiBaseView baseView)
        {
            baseView.Canvas.enabled = true;
            baseView.CanvasGroup.alpha = 0;
            if (!baseView.Canvas.gameObject.activeInHierarchy)
                baseView.Canvas.gameObject.SetActive(true);

            return baseView.CanvasGroup.DOFade(1, baseView.FadeInDuration / 1000f);
        }

        public static DG.Tweening.Tween TweenFadeOut(this UiBaseView baseView, Action onComplete )
        {
            return baseView.CanvasGroup.DOFade(0, baseView.FadeOutDuration / 1000f)
                .OnComplete(() =>
                {
                    baseView.Canvas.enabled = false;
                    onComplete?.Invoke();
                });
        }
    }
}