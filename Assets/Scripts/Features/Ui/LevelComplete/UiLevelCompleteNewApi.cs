
using System;
using System.Threading.Tasks;
using Data;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;
using Utils.DoTween;

namespace Components
{
    [Serializable]
    public class UiLevelCompleteNewApi : UiBaseApi
    {
        private UiLevelCompleteView _view;
        private ProfileData _profile;
        private bool _done;
        
        public void SetCtx(UiLevelCompleteView view, CoreRoot root)
        {
            _view = view;
            _profile = root.Data.Profile;
            SetCtxBase(view);
            
            _view.ContinueButton.onClick.AddListener(Close);
        }

        [Button]
        public async void TestShow()
        {
            await Show(50, 10);
        }

        public async Task Show(int levelReward, int movesReward)
        {
            _view.CoinsCount.text = $"{_profile.Coins}";
            _view.HeartsCount.text = $"{_profile.Hearts}";

            // _view.LevelCompleteHeader.gameObject.SetActive(false);
            _view.RewardForLevel.gameObject.SetActive(false);
            _view.RewardForMoves.gameObject.SetActive(false);
            _view.CoinsForLevel.Text.text = "";
            _view.CoinsForMoves.Text.text = "";
            _view.LevelCompleteHeader.HideImmediate();

            var buttonTransform = _view.ContinueButton.GetComponent<RectTransform>();

            await DoShow();

            var totalCoins = _profile.Coins + levelReward + movesReward;
            var seq = DOTween.Sequence();
            
            seq.InsertCallback(0, _view.LevelCompleteHeader.ShowText);

            var offset = -0.2f;
            seq.InsertCallback(1f + offset, () => _view.RewardForLevel.gameObject.SetActive(true));
            seq.InsertCallback(1.6f + offset, () => _view.RewardForMoves.gameObject.SetActive(true));

            seq.InsertCallback(1.25f + offset, () => _view.CoinsCount.DoInt(totalCoins, 1.4f + 0.4f));
            seq.InsertCallback(1.25f + offset, () => 0.DoInt(levelReward, 1.4f, count => _view.CoinsForLevel.Text.text = $"+ {count}") );
            seq.InsertCallback(1.85f + offset, () => 0.DoInt(movesReward, 1.4f, count => _view.CoinsForMoves.Text.text = $"+ {count}") );
            
            seq.InsertCallback(1.85f  + offset + _view.ButtonDelay, () => buttonTransform.DOShakeScale(_view.ShakeDuration, _view.ShakeStrength, _view.ShakeVibrato));
            

            var done = false;
            seq.OnComplete(() => done = true);

            _done = false;
            while (_done == false)
                await Task.Yield();
            
            seq.Kill();

            DoHide().DoAsync();
        }

        private void Close()
        {
            _done = true;
        }
    }
}