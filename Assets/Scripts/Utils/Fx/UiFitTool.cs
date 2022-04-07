using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Components.Tools
{
    public class UiFitTool : MonoBehaviour
    {
        public Image Target;
        public SpriteRenderer Source;
        
        public Camera CoreCamera;
        public bool UseScale;
        public bool IgnoreDuringShake;
        public float Strength = 1f;
        public float Duration = 0.5f;

        
        [Button]
        void Fit()
        {
            Source.DoFxFly(CoreCamera, Target.transform as RectTransform, true, () => { }).DoAsync();
            
            var fx = new GameObject("fx");
            var image = fx.AddComponent<Image>();
            // var tr = fx.GetComponent<RectTransform>();
            // tr.FitTransform((RectTransform) Target.transform);
            // tr.SetParent(Target.transform.parent, true);
            image.FitUiByObject(Source, CoreCamera);
            //
            // tr.DOScale(Target.transform.localScale, 1f).SetEase(Ease.InExpo);
            // tr.DOMoveX(Target.transform.UiCenter().x, 1f).SetEase(Ease.InOutSine);
            // tr.DOMoveY(Target.transform.UiCenter().y, 1f).SetEase(Ease.Linear).OnComplete(() =>
            // {
            //     image.color = new Color(1, 1, 1, 0);
            //     fx.DoDelayedDestroy(500);
            //     
            //     (Target.transform as RectTransform).DOKill();
            //     (Target.transform as RectTransform).DOShakeScale(0.5f, 0.3f);
            // });

            // fx.AddComponent<Image>();
            // fx.AddComponent<RectTransform>()
            // target.FitUiByObject(Source, CoreCamera);

            // var diff = CoreCamera.UnitSizeOnUi();
            // target.transform.position = Source.transform.position.WorldToScreen(CoreCamera);
            // var size = target.sizeDelta;
            // if(UseScale)
            // target.localScale = new Vector3(diff.x / size.x, diff.y / size.y );
            // else
            // target.sizeDelta = diff;
        }
    }
}