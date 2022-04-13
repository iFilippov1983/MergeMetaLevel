using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    public static class FxExtensions
    {
        public static async Task<GameObject> DoFxFyFrom(
            this Image target
            , Vector3 fromPosition
            ,float duration
            , AnimationCurve curveX
            , AnimationCurve curveY
            , bool doShake
            , Action cbComplete)
        {
            var fx = new GameObject("fx");
            var image = fx.AddComponent<Image>();
            var tr = fx.GetComponent<RectTransform>();
            
            var targetTransform = (RectTransform) target.transform;
            
            tr.SetParent(targetTransform.parent, true);
            tr.sizeDelta = targetTransform.sizeDelta;
            tr.position = fromPosition;
            image.sprite = target.sprite;
            tr.localScale = new Vector3(0.2f, 0.2f, 1);
            
            
            var done = false;
            tr.DOScale(1, duration).SetEase(Ease.OutExpo);
            tr.DOMoveX(targetTransform.UiCenter().x, duration).SetEase(curveX);
            tr.DOMoveY(targetTransform.UiCenter().y, duration).SetEase(curveY).OnComplete(() =>
            {
                // targetTransform.DOKill();
                if (doShake)
                {
                    targetTransform.DOKill();
                    targetTransform.localScale = Vector3.one;
                    targetTransform.DOShakeScale(0.6f, 0.4f);
                    // tr.DOScale(1.15f, 0.15f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutQuad);
                    // tr.DOShakeScale(0.8f, 0.2f);
                    // tr.DOPunchScale(new Vector3(1,1,0), 0.5f);
                    // Async.DelayedCall(800,() => image.color = new Color(1, 1, 1, 0));
                    // fx.DoDelayedDestroy(900);
                    fx.DoDestroy();
                }
                else
                    fx.DoDestroy();

                cbComplete();
                done = true;
            });

            while (!done)
                await Task.Yield();
            
            return fx;
        }

        public static async Task<GameObject> DoFxFlyStar(this SpriteRenderer Source, Camera camera, RectTransform target, bool doShake, Action cbComplete, int destroyDelay = 500)
        {
            var targetTransform = (RectTransform) target.transform;
            
            var fx = new GameObject("fx");
            var image = fx.AddComponent<Image>();
            var tr = fx.GetComponent<RectTransform>();
            
            tr.FitTransform(targetTransform);
            tr.SetParent(targetTransform.parent, true);
            image.FitUiByObject(Source, camera);

            tr.DOScale(1, 1f).SetEase(Ease.InExpo);
            tr.DOMoveX(targetTransform.UiCenter().x, 1f).SetEase(Ease.InOutSine);
            var done = false;
            tr.DOMoveY(targetTransform.UiCenter().y, 1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                // (target.transform as RectTransform).DOKill();
                if (doShake)
                {
                    tr.DOShakeScale(0.8f, 0.3f);
                    // tr.DOScale(1.1f, 0.15f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutQuad);
                    Async.DelayedCall(destroyDelay,() => image.color = new Color(1, 1, 1, 0));
                    fx.DoDelayedDestroy(destroyDelay);
                }
                else
                    fx.DoDestroy();
                
                cbComplete();
                done = true;
            });

            while (!done)
                await Task.Yield();
            
            return fx;
        }
        
        public static async Task<GameObject> DoFxFly(this SpriteRenderer Source, float duration,  Camera camera, RectTransform target, bool doShake, Action cbComplete, int destroyDelay = 500)
        {
            var targetTransform = (RectTransform) target.transform;
            
            var fx = new GameObject("fx");
            var image = fx.AddComponent<Image>();
            var tr = fx.GetComponent<RectTransform>();
            
            tr.FitTransform(targetTransform);
            tr.SetParent(targetTransform.parent, true);
            image.FitUiByObject(Source, camera);

            tr.DOScale(1, duration).SetEase(Ease.InExpo);
            tr.DOMoveX(targetTransform.UiCenter().x, duration).SetEase(Ease.InOutSine);
            var done = false;
            tr.DOMoveY(targetTransform.UiCenter().y, duration).SetEase(Ease.Linear).OnComplete(() =>
            {
                // (target.transform as RectTransform).DOKill();
                if (doShake)
                {
                    target.localScale = Vector3.one;
                    target.DOKill();
                    target.DOShakeScale(0.6f, 0.4f);
                    // tr.DOScale(1.1f, 0.15f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutQuad);
                    // Async.DelayedCall(destroyDelay,() => image.color = new Color(1, 1, 1, 0));
                    fx.DoDelayedDestroy(0);
                }
                else
                    fx.DoDestroy();
                
                cbComplete();
                done = true;
            });

            while (!done)
                await Task.Yield();
            
            return fx;
        }
        
        public static Vector2 UnitSizeOnUi(this Camera camera)
        {
            var zero = camera.WorldToScreenPoint(Vector3.zero);
            var one = camera.WorldToScreenPoint(new Vector3(1, 1));
            var diff = new Vector2(one.x - zero.x, one.y - zero.y);
            return diff;
        }

        public static void FitTransform(this RectTransform ui, RectTransform source)
        {
            ui.sizeDelta = source.sizeDelta;
            ui.localScale = source.localScale;
            ui.position = source.position;
        }
        
        public static void FitUiByObject(this Image ui, SpriteRenderer obj, Camera camera, bool useScale = true)
        {
            var tr = obj.transform;
            var uiTr = ui.GetComponent<RectTransform>();
            uiTr.FitPosition(tr, camera);
            uiTr.FitScale(tr, camera);
            ui.FitSprite(obj);
        }

        public static void FitSprite(this Image ui, SpriteRenderer spriteRenderer)
        {
            ui.sprite = spriteRenderer.sprite;
        }
        
        public static void FitPosition(this RectTransform ui, Transform tr, Camera camera)
        {
            ui.position = tr.position.WorldToScreen(camera);
        }

        public static void FitSize(this RectTransform ui, Transform tr, Camera camera)
        {
            var diff = camera.UnitSizeOnUi();
            var targetSize = diff  * tr.localScale;
            ui.sizeDelta = targetSize;
        }
        
        public static void FitScale(this RectTransform ui, Transform tr, Camera camera)
        {
            var diff = camera.UnitSizeOnUi();
            var targetSize = diff  * tr.localScale;
            var uiSize = ui.sizeDelta;
            ui.localScale = new Vector3(targetSize.x / uiSize.x, targetSize.y / uiSize.y, 1);
        }
    }
}