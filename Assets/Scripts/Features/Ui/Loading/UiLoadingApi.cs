using System;
using System.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Components
{
    [Serializable]
    public class UiLoadingApi : UiBaseApi
    {
        private UiLoadingView _view;

        public void SetCtx(UiLoadingView view)
        {
            _view = view;
            base.SetCtxBase(view);
            view.Canvas.enabled = true;
            view.gameObject.SetActive(false);
            
            // _view.Canvas.enabled = false;
        }

        [Button]
        public async Task Show()
        {
            _view.gameObject.SetActive(true);
            await DoShow();
            await Task.Delay(_view.ShowDuration );
            // await Task.Delay(200);
        }
        
        // [Button]
        // public async Task Show()
        // {
        //     if(!_view.gameObject.activeInHierarchy)
        //         _view.gameObject.SetActive(true);
        //     
        //     var newVal = _view.Bg.sizeDelta;
        //     newVal.y = -_view.RootCanvasRect.rect.height;
        //     _view.Bg.sizeDelta = newVal;
        //     _view.Canvas.enabled = true;
        //     
        //     _view.Bg.DOSizeDelta(Vector2.zero, 800f/1000f).SetEase(Ease.Linear);
        //     await Task.Delay(800);
        // }
        //
        [Button]
        public async Task Hide()
        {
            await DoHide();
            _view.gameObject.SetActive(false);

            // var newVal = _view.Bg.sizeDelta;
            // newVal.y = -_view.RootCanvasRect.rect.height;
            // _view.Bg.DOSizeDelta(newVal, 600f/1000f).SetEase(Ease.Linear);
            //
            // await Task.Delay(600);
            // _view.Canvas.enabled = false;
        }
    }
}