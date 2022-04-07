using System;
using System.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Components
{
    [Serializable]
    public class UiBlurApi : UiBaseApi
    {
        private UiBlurView _view;
        private Image _renderer;
        public float _alpha;
        public float _opacity;

        public void SetCtx(UiBlurView view)
        {
            _view = view;
            _renderer = _view.GetComponent<Image>();
            
            base.SetCtxBase(view);
            view.Canvas.enabled = true;
            view.gameObject.SetActive(false);

            view.OnUpdate += Update;
        }
        
        [Button]
        public async Task Show()
        {
            // _alpha = 0f;
            _renderer.materialForRendering.SetFloat("_Alpha", _alpha);
            _view.gameObject.SetActive(true);
            DOTween.To(() => _alpha, v => _alpha = v, 1f, _view.FadeInDuration / 1000f);
            await Task.Delay(_view.ShowAwaitTime);
        }

        [Button]
        public async Task Hide()
        {
            // await Task.Delay(_view.HideDelayTime);
            DOTween.To(() => _alpha, v => _alpha = v, 0f, _view.FadeOutDuration / 1000f);
            await Task.Delay(_view.FadeOutDuration);
            _view.gameObject.SetActive(false);
        }

        void Update()
        {
            // _renderer.materialForRendering.SetFloat("Alpha", _alpha);
            _renderer.materialForRendering.SetFloat("_Alpha", _alpha);
        }
    }
}