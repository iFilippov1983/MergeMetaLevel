using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Components
{
    [Serializable]
    public class UiWellDoneApi : UiBaseApi
    {
        private UiWellDoneView _view;

        public void SetCtx(UiWellDoneView view, CoreRoot root)
        {
            SetCtxBase(view);
            _view = view;
            _view.RootCanvasRect = root?.Ui.View.GetComponent<RectTransform>();
        }
        
        public async Task Show(float k = 1f)
        {
            _view.Confety.SetActive(true);
            ShowImmediately();
            var pos = _view.Container.anchoredPosition;
            pos.y = _view.RootCanvasRect.rect.height * k;
            _view.Container.anchoredPosition = pos;
            _view.Container.DOAnchorPosY(0, 1.2f).SetEase(Ease.OutBack);
            await Task.Delay(1200);
        }
        
        public async Task Hide(float k = 1f)
        {
            var posY = _view.RootCanvasRect.rect.height * k;
            _view.Container.DOAnchorPosY(posY, 1.2f).SetEase(Ease.InBack);
            await Task.Delay(1200);
            _view.Confety.SetActive(false);
            HideImmediately();
        }
        
        
    }
}