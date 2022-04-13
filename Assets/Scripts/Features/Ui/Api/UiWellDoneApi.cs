using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Utils;

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
            _view.ContainerGroup.alpha = 1;
            _view.Confety.SetActive(true);
            ShowImmediately();
            var pos = _view.Container.anchoredPosition;
            pos.y = _view.RootCanvasRect.rect.height * k;
            _view.Container.anchoredPosition = pos;
            _view.Container.DOAnchorPosY(0, 0.8f).SetEase(Ease.OutBack);
            await Task.Delay(800);
        }
        
        public async Task Hide(float k = 1f)
        {
            // await DoHide();
            await _view.ContainerGroup.DoFadeOut(_view.FadeOutDuration);
            // var posY = _view.RootCanvasRect.rect.height * k;
             // _view.Container.DOAnchorPosY(posY, 0.9f).SetEase(Ease.InBack);
             ConfetyHide().DoAsync();
        }

        private async Task ConfetyHide()
        {
            await Task.Delay(800);
            _view.Confety.SetActive(false);
            HideImmediately();
        }
    }
}