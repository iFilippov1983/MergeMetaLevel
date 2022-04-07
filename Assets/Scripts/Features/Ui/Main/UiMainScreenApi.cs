using System;
using System.Threading.Tasks;
using Core;
using Data;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace Components
{
    [Serializable]
    public class UiMainScreenApi : UiBaseApi
    {
        private UiMainScreenView _view;
        private CoreRoot _root;
        private bool _locked;
        private ProfileData _profileData;
        public event Action OnPlayBtnClick;
        public event Action OnSettingsBtnClick;

        public event Action OnHearts;
        public event Action OnCoins;
        public event Action OnDiamonds;
        public event Action OnStars;

        public void SetCtx(UiMainScreenView view, CoreRoot root)
        {
            _view = view;
            _root = root;
            _profileData = root.Data.Profile;
            
            _view.PlayBtn.OnClick(() => Invoke(OnPlayBtnClick));
            _view.SettingsBtn.OnClick(() => Invoke(OnSettingsBtnClick));
            
            _view.HeartsBtn.OnClick += (() => Invoke(OnHearts));
            _view.CoinsBtn.OnClick +=(() => Invoke(OnCoins));
            _view.DiamondsBtn.OnClick+=(() => Invoke(OnDiamonds));
            _view.XpBtn.OnClick += (() => Invoke(OnStars));

            root.Events.Data.OnResourceChanged += OnResourceChanged;
            
            SetCtxBase(view);
        }

        private void OnResourceChanged(ResourceType type, int value, int oldValue)
        {
            if(type == ResourceType.Hearts)
                _view.HeartsBtn.Count = value;
        }


        private void Invoke(Action action)
        {
            if(!_locked)
                action?.Invoke();
        }

        protected override void BeforeShow()
        {
            base.BeforeShow();
            RedrawView();
            _view.Levels.Init(_profileData.LevelIndex + 1);
        }

        public void ShowPanels()
            => _view.CanvasGroup.DoFadeIn(400).DoAsync();
        
        public void HidePanels()
            => _view.CanvasGroup.DoFadeOut(400).DoAsync();

        public void RedrawView()
        {
            SetHears();
            SetCoins();
        }

        public Transform GetXpCenter() => _view.StarsIcon;
        private void SetHears() => _view.HeartsBtn.Count = _profileData.Hearts;
        private void SetCoins() => _view.CoinsBtn.Count = _profileData.Coins;

        public async Task XpFly() => await _view.XpFly.Test_Fly();

        public void Lock(bool locked) 
            => _locked = locked;

        [Button]
        public async Task MoveLevels()
        {
            await _view.Levels.Move();
            
            await Task.Delay(600);
            var buttonTransform = _view.PlayBtn.GetComponent<RectTransform>();
            buttonTransform.DOShakeScale(0.6f, 0.2f, 4);
        }
    }
}