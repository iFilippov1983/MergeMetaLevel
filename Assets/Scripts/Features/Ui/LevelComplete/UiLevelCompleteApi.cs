
using System;
using System.Threading.Tasks;
using Data;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;
using Utils.DoTween;
using Utils.TimelineTools;

namespace Components
{
    [Serializable]
    public class UiLevelCompleteApi : UiBaseApi
    {
        private UiLevelComplete _view;
        private bool _clicked;
        private ProfileData _profile;
        private int _coinsReward;
        private int _xpReward;
        
        public UiLevelComplete view => _view;

        public void SetCtx(UiLevelComplete view, CoreRoot root)
        {
            _view = view;
            SetCtxBase(view);
            _view.OnClick += OnPointerDown;
            _profile = root.Data.Profile;
        }

        protected override void BeforeShow()
        {
            base.BeforeShow();
        }
        

        [Button]
        public async void TestShow()
        {
            await Show(50, 1);
        }

        public Task ShowBg() => DoShow();
        public Task ShowContent() => _view.Content.DoFadeIn(_view.FadeInDuration);
        public void WellDone_Show() => _view.WellDoneText.ShowText();
        public void WellDone_Hide() => _view.WellDoneText.HideText();
        public async void Star_Fly() => await XpFly();
        public void Coins_Add() => _view.CoinsMono.DoCount(_coinsReward);
        public void Coins_Fly()
        {
        }

        public void Coins_FlyEnd()
        {
            _view.CoinsCount.DoInt(_profile.Coins + _coinsReward, 0.8f);
            _view.XpCount.DoInt(_profile.Xp + _xpReward, 0.4f);
        }

        public async void WaitForTap()
        {
            _view.PlayableDirector.Pause();
            await WaitTap();
            _view.PlayableDirector.Resume();
        }


        public async Task Show(int coinsReward, int xpReward)
        {
            _coinsReward = coinsReward;
            _xpReward = xpReward;
            
            // _view.CoinsMono.Text.text = $"{_profile.Coins}";
            _view.CoinsCount.text = $"{_profile.Coins}";
            _view.XpCount.text = $"{_profile.Xp}";
                
            ShowImmediately();
            _view.PlayableDirector.Play();
            await _view.PlayableDirector.Wait();
            
            // _view.Content.alpha = 0;
            // _view.TapToContinue.alpha = 0;
            // await ShowBg();
            // await Task.Delay(_view.BgDelay);
            // await ShowContent();
            // await Task.Delay(100);
            //
            // await XpFly();
        }
        

        private Transform GetXpCenter() => _view.XpIcon;
        private void SetXp(ProfileData profileData) => _view.XpCount.text = $"{profileData.Xp}";

        private void SetXp_MinusOne(ProfileData profileData) =>
            _view.XpCount.text = $"{profileData.Xp - 1}";

        private async Task XpFly()
        {
            _view.StarFlyTween.DoTween(0);
            // await _view.XpFly.Test_Fly();
        }

        public void OnPointerDown()
        {
            _clicked = true;
            Debug.Log("OnPointerDown");
        }
        
        private async Task WaitTap()
        {
            _clicked = false;
            await _view.TapToContinue.DoFadeIn(300);
            while (!_clicked)
                await Task.Delay(50);
            await _view.TapToContinue.DoFadeOut(300);
        }
    }
}