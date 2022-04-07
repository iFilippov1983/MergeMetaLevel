using System;
using System.Threading.Tasks;
using UnityEngine.UI;
using Utils;

namespace Components
{
    [Serializable]
    public class UiBaseApi
    {
        protected bool _closed;
        protected bool _outerCommand;

        private bool _hasCloseBtn;
        private bool _wasInit;
        private UiBaseView _baseView;
        
        // Api
        
        public void SetCtxBase(UiBaseView view)
        {
            _baseView = view;
            HideImmediately();
        }
        
        void RuntimeInit()
        {
            if (!_wasInit)
                Init();
            _wasInit = true;
        }
        protected virtual void Init()
        {
            
        }
        
        protected void SetCloseButton(Button closeBtn)
        {
            closeBtn.OnClick(SetClosed);
            _hasCloseBtn = true;
        }

        protected void SetClosed()
        {
            _closed = true;
        }

        protected virtual void BeforeShow()
        {
            
        } 
        protected virtual void AfterShow()
        {
            
        }

        public async Task HideOnCloseClick()
        {
            if (!_hasCloseBtn)
                throw new Exception("SetCloseButton must be called in Init().");
            await WaitForClose();
            await DoHide();
        }
        

        public async Task WaitUntil(Func<bool> cb)
        {
            while (!cb())
                await Task.Yield();
        }

        public async Task WaitForClose()
        {
            _closed = false;
            while (!_closed)
                await Task.Yield();
        }

        public void ShowImmediately()
        {
            RuntimeInit();

            BeforeShow();

            _closed = false;
            _baseView.CanvasGroup.interactable = true;
            _baseView.Canvas.enabled = true;
            _baseView.CanvasGroup.alpha = 1;
            
            if(!_baseView.gameObject.activeInHierarchy)
                _baseView.gameObject.SetActive(true);
            
            AfterShow();
        }
        
        public void HideImmediately()
        {
            _closed = true;
            _baseView.CanvasGroup.interactable = false;
            _baseView.Canvas.enabled = false;
            _baseView.CanvasGroup.alpha = 0;
        }

        protected async Task ShowThenClose()
        {
            await DoShow();
            
            if (!_hasCloseBtn)
                throw new Exception("SetCloseButton must be called in Init().");
            
            await WaitForClose();
            await DoHide();
        }

        public async Task DoShow()
        {
            RuntimeInit();

            BeforeShow();

            await ShowBlur();
            _baseView.gameObject.SetActive(true);
            await _baseView.DoFadeIn();
            
            AfterShow();
        }

        public async Task DoHide(bool hideBlur = true)
        {
            await _baseView.DoFadeOut();
            
            if(hideBlur)
                await HideBlur();
        }

        protected UiBlurView SetBlur(CoreRoot root)
        {
            return _baseView.Blur = root.Ui.View.Blur;
        }

        protected async Task ShowBlur()
        {
            if (_baseView.Blur != null)
                await _baseView.Blur.Api.Show();
        }

        protected async Task HideBlur()
        {
            if (_baseView.Blur != null)
                await _baseView.Blur.Api.Hide();
        }
    }
}