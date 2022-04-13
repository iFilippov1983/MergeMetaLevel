using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Random = UnityEngine.Random;

namespace Components
{
    [Serializable]
    public class UiMergeViewApi : UiBaseApi
    {
        public event Action OnPauseBtnClick;
        public event Action OnQuestsBtnClick;
        public event Action<string> OnToolClick;

        private UiMergeView _view;
        private MergeVisualConfig _visualConfig;
        public UiMergeView View => _view;


        // Api
        public void SetCtx(UiMergeView view, CoreRoot root)
        {
            _view = view;

            _visualConfig = root.Configs.Merge.VisualConfig;

            base.SetCtxBase(_view);
            _view.CanvasGroup.interactable = true;
            _view.Canvas.enabled = true;
            
            _view.BackBtn.OnClick(() => OnPauseBtnClick?.Invoke());
            _view.QuestsBtn.OnClick(() => OnQuestsBtnClick?.Invoke());

            foreach (var toolButton in _view.ToolButtons)
                toolButton.button.OnClick(() => OnToolClick?.Invoke(toolButton.name));
        }

        public void Show(LevelConfig level, int moves, int xp, int levelIndex)
        {
            // WellDoneHideImmediately();
            UpdateGoals(level);
            UpdateMoves(moves);
            UpdateXp();
            UpdateLevel(levelIndex);
            _view.UiGo.gameObject.SetActive(false);
            
            HideCoins();
            ShowGoals(level);
        }

        private void HideCoins()
        {
            _view.CoinsGroup.alpha = 0;
        }
        
        private void ShowGoals(LevelConfig level)
        {
            _view.GoalImg.sprite = level.Goal.Config.Sprite;
            _view.GoalImg.color = Color.white;
        }
        
        public void SwapGoalsCoins(int coins)
        {
            _view.CoinsText.text = coins.ToString();
            _view.GoalImg.DoFadeOut(1200).DoAsync();
            _view.CoinsGroup.DoFadeIn(1200).DoAsync();
        }

        public void UpdateXp()
        {
            // _view.XpCount.text = $"x{_profileData.Xp}";
        }

        public Task WaitGoComplete() 
            => View.GoReadyEvent.WaitEvent1();

        public void ShowEditorAlarm(bool show) => _view.EditorActive.gameObject.SetActive(show);

        public async Task CoinsFromMovesFly(int movesCount, Action cbAddCoin)
        {
            var tasks = new List<Task>();
            var max = movesCount;
            var totalTime = _visualConfig.CoinsFromMovesDelay;
            var nextCoinDelay = totalTime / (max - 1);
            for (int i = 0; i < max; i++)
            {
                movesCount--;
                UpdateMoves(movesCount);
                    
                var keys = _visualConfig.CoinsFromMovesY.keys;
                keys[1].value *= _visualConfig.CoinsFromMovesHeight * (i % 2 == 0 ? 1 : -1);
                var duration = 0.45f;
                var task = View.CoinsImage.DoFxFyFrom( View.MovesPos.position, duration,  _visualConfig.CoinsFromMovesX, new AnimationCurve(keys), true, cbAddCoin);
                tasks.Add(task);
                await Task.Delay(nextCoinDelay);
            }

            await Task.WhenAll(tasks);
        }

        [Button]
        void CoinsFromMovesFly()
            => CoinsFromMovesFly(10, () => { }).DoAsync();
        
        private void UpdateLevel(int levelIndex)
        {
            _view.LevelTxt.text = (levelIndex +1).ToString();
        }

        private void UpdateGoals(LevelConfig level)
        {
            _view.GoalImg.sprite = level.Goal.Config.Sprite;
        }

        public void UpdateMoves(int mergeLevelMoves)
        {
            _view.MovesTxt.text = mergeLevelMoves.ToString();
        }

        public void ShowGO()
        {
            _view.UiGo.gameObject.SetActive(true);
        }

    }
}