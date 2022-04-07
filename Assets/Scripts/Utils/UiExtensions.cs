using System.Collections.Generic;
using System.Threading.Tasks;
using Components;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Utils
{
    public static class UiExtensions
    {
        public static void OnClick(this Button btn, UnityAction cb)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(cb);
        } 
        
        public static void PopulateFromPrefab<T>( int needCount, Transform container, T prefab, ref List<T> items) where T : MonoBehaviour
        {
            // if (!items.Contains(prefab))
            //     items.Add(prefab);
                
            if(needCount > items.Count )
                while (items.Count < needCount)
                {
                    var newItem = Object.Instantiate(prefab, container);
                    items.Add(newItem);
                }
            
            items.ForEach(item => item.gameObject.SetActive(false));
        }
        
        public static Vector3 UiCenter(this RectTransform transform)
            => transform.TransformPoint(transform.rect.center);

        public static Vector3 UiCenter(this Transform transform)
            => UiCenter(transform as RectTransform);
        
        public static async Task DoFadeIn(this UiBaseView baseView)
        {
            baseView.CanvasGroup.interactable = true;
            baseView.Canvas.enabled = true;
            if (!baseView.Canvas.gameObject.activeInHierarchy)
                baseView.Canvas.gameObject.SetActive(true);

            baseView.CanvasGroup.alpha = 0;
            baseView.CanvasGroup.DOFade(1, baseView.FadeInDuration / 1000f);
            await Task.Delay(baseView.FadeInDuration);
        }

        public static async Task DoFadeOut(this UiBaseView baseView)
        {
            if(baseView.Canvas.enabled == false)
                return;
            
            baseView.CanvasGroup.interactable = false;
            baseView.CanvasGroup.DOFade(0, baseView.FadeOutDuration/1000f);
            await Task.Delay(baseView.FadeOutDuration);
            
            baseView.Canvas.enabled = false;
        }

        public static async Task DoFadeIn(this Image image, int duration)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
            image.DOFade(1, duration/1000f);
            await Task.Delay(duration);
        }
        public static async Task DoFadeOut(this Image image, int duration)
        {
            image.DOFade(0, duration/1000f);
            await Task.Delay(duration);
        }
        
        public static async Task DoFadeIn(this CanvasGroup canvasGroup, int duration)
        {
            canvasGroup.alpha = 0;
            canvasGroup.DOFade(1, duration/1000f);
            await Task.Delay(duration);
        }
        public static async Task DoFadeOut(this CanvasGroup canvasGroup, int duration)
        {
            canvasGroup.DOFade(0, duration/1000f);
            await Task.Delay(duration);
        }
    }
}