using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Merge;
using Components;
using Configs;
using Configs.Meta;
using Configs.Tutorial;
using Core;
using Data.Dynamic;
using Features._Events;
using UnityEngine;
using Utils;

namespace Data
{
    enum ActiveState
    {
        None,
        Player,
        Core
    }
    public class MergeApi
    {
        // private RootCtx _rootCtx;
        private MergeView _view;
        private MergeDynamicData _data;
        private MergeEvents _events;
        private MergeConfig _config;
        private UiApi _ui;

        private Core.Core _core;
        private MergeEditor _editor;

        private bool _allGoalsDone;
        private bool _isActive;
        private ActiveState _activeState;

        private bool _gameSuccess => _allGoalsDone == true;
        private bool _isClickAllowed => _allGoalsDone == false && _data.PlayerMoves > 0;
        private bool _levelComplete;
        private ProfileData _profile;
        private TutorialEvents _tutorialEvents;
        private ShopConfig _staticDataShop;
        private DebugConfig _debugConfig;
        private Camera _camera;
        private MergeVisualConfig _visualConfig;
        private UiMergeView _mergeView;


        public void SetCtx(MergeView view, MergeDynamicData data, MergeEvents events, TutorialEvents tutorialEvents, MergeConfig config,
            ShopConfig staticDataShop,
            ProfileData profile, UiApi ui, CoreRoot root)
        {
            _ui = ui;
            _view = view;
            _data = data;
            _events = events;
            _config = config;
            _profile = profile;
            _editor = _view.Editor;
            _visualConfig = root.Configs.Merge.VisualConfig;
            _tutorialEvents = tutorialEvents;
            _staticDataShop = staticDataShop;
            _debugConfig = root.Configs.Debug;
            _camera = root.View.Merge.Player.Camera;
            
            // _ui.Merge.OnQuestsBtnClick += SwitchEditorPlayer;
            _editor.SetCtx();
        }

        public void OnUpdate()
        {
            if(!_isActive)
                return;

            _core?.Execute();
            // _editor.OnUpdate();
        }

        public void SetActive()
        {
            _isActive = true;
            _activeState = ActiveState.Player;
            
            _view.gameObject.SetActive(true);
            ActivateEditor(false);
            // ActivateEditorPlayer(true);
        }

        public void SetInactive()
        {
            _isActive = false;
            _activeState = ActiveState.None;

            ActivatePlayer(false);
            ActivateEditor(false);
            _view.gameObject.SetActive(false);
        }

        public async Task<bool> PlayLevel()
        {
            _data.Input.InputLocked = false;
            _tutorialEvents?.Check?.Invoke(TutorialTriggerType.Gameplay);

            while (true)
            {
                await WaitLevelComplete();

                if (!_gameSuccess)
                {
                    var buy = await BuyMoves();
                    if (buy)
                    {
                        if(EnoughToBuyMoves() || _debugConfig.FreeBuy)
                        {
                            _profile.Coins -= _staticDataShop.BuyMovesCost;
                            await AddMovesAndBonuses();
                            continue;
                        }
                    }
                    // await OnLevelFail();
                    // break;
                }
                
                // if (_gameSuccess)
                // await OnLevelWin();
                
                break;
            }

            _data.Input.InputLocked = true;
            return _gameSuccess;
        }

        private bool EnoughToBuyMoves()
        {
            return _profile.Coins >= _staticDataShop.BuyMovesCost;
        }

        public void SetLevelIfNull(int profileLevelIndex)
        {
            if (_data.Level == null)
            {
                if (profileLevelIndex >= _config.Levels.Count)
                    profileLevelIndex = 0;
                
                _data.Level = _config.Levels[profileLevelIndex];
            }
        }

        public void SetLevelByIndex(int levelIndex)
        {
            _data.Level = _config.Levels[levelIndex];
        }
        
        private void CoreCreate() 
            => _core = _core 
                       ?? new Core.Core(Contexts.sharedInstance, _view.Player, _config, _data, _events);

        // private void CoreNewGame(LevelConfig levelConfig) 
        //     => _core?.NewGame(levelConfig);
        //
        // private void CoreUpdate() 
        //     => _core?.Execute();

        private void CoreClear() 
            => _core?.Clear();

        private void CoreRelease()
        {
            _core?.TearDown();
            _core = null;
        }

        private void CheckWinOrFail()
        {
            CheckGoals();
            
            var levelComplete = _allGoalsDone || _data.PlayerMoves <= 0;
            _levelComplete = levelComplete;
        }

        private async Task<int> FinalClear(int coinsReward)
        {
            var coins = coinsReward;
             
            void AddCoin()
            {
                ++coins;
                _ui.Merge.View.CoinsText.text = coins.ToString();
            }
             
            // void Fly(MergeItemView itemView) 
            // => itemView.SpriteRenderer.DoFxFly(_camera, coinsTransform, AddCoin);

            // var moves = _ui.Merge.View.MovesPos as RectTransform;
            // var coinsUi= _ui.Merge.View.CoinsImage;
            // var goalImg = _ui.Merge.View.GoalImg;
            //      
            // _ui.Merge.View.GoalImg.DoFadeOut(1200).DoAsync();
            // _ui.Merge.View.CoinsGroup.DoFadeIn(1200).DoAsync();
            // _ui.Merge.View.CoinsText.text = coins.ToString();
            
            _ui.Merge.SwapGoalsCoins(coins);
             
            var coinsImg= _ui.Merge.View.CoinsImage;
            await _core.Systems.tools.FinalClear( coinsImg.transform as RectTransform, AddCoin);
            // input.SkipPressed = false;

            await Task.Delay(200);

            await _ui.Merge.CoinsFromMovesFly(_data.PlayerMoves, AddCoin);
            _data.PlayerMoves = 0;
             
            // await Task.Delay(200);
             
            // var uiStar = _ui.LevelComplete.view.XpIcon;
            // var starView = _core.Systems.tools.FindStar().view.value.SpriteRenderer;
            // starView.color = Color.white.NoAlpha();
            // starView.DoFxFlyStar(_camera, uiStar, true, () => { }, _visualConfig.CoinsStarDestroyDelay).DoAsync();
             
            // Async.DelayedCall(2000, () =>
            // {
            //     _ui.Merge.View.GoalImg.color = Color.white;
            //     _ui.Merge.View.CoinsGroup.alpha = 0;
            // });

            return coins;
        }

        public void Cheat_WinLevel() 
        {
            _levelComplete = true;
            _allGoalsDone = true;
        }
        public void Cheat_FailLevel() 
        {
            _levelComplete = true;
            _allGoalsDone = false;
        }
        
        private void CheckGoals()
        {
            var level = _data.Level;
            var goal = level.Goal;

            var countOnBoard = _core.ChipsCount(goal.Config);
            // var goals = _data.Items.FindAll(v => v.Config == goal.Config);
            if (countOnBoard >= goal.Count)
                _allGoalsDone = true;
        }

        private void UpdateMoves()
        {
            _data.PlayerMoves--;
            _ui.Merge.UpdateMoves(_data.PlayerMoves);
        }

        private void ActivatePlayer(bool activate)
        {
            if (activate)
            {
                CoreCreate();
            }
            else
            {
                CoreClear();
                CoreRelease();
            }
            // _core.SetActive(activate);
        }

        private void ActivateEditor(bool activate)
        {
            _editor.gameObject.SetActive(activate);
            _ui.Merge.ShowEditorAlarm(activate);
        }

        public void PreparePlayerLevel()
        {
            var level = _data.Level;

            CoreCreate();
            
            _data = _core.data;
            
            _data.Input.InputLocked = true;
            _data.PlayerMoves = level.Moves;
            _allGoalsDone = false;
            // _levelComplete = false;

            _ui.LevelComplete.HideImmediately();
            _ui.WellDone.HideImmediately();
            _ui.Merge.Show(level, _data.PlayerMoves, _profile.Xp, _config.Levels.IndexOf(level));
            
            // _events.OnFieldChanged = CheckGoals;
            _events.OnMoveSpend = UpdateMoves;
            _events.OnStepEnd = CheckWinOrFail;
            _events.IsClickAllowed = (x,y) => _isClickAllowed;
            
            _core.NewGame(level);
            
            _ui.Merge.ShowGO();
        }

        private async Task<bool> BuyMoves()
        {
            var buy = await _ui.OutOfMoves.Show();
            if (buy && !EnoughToBuyMoves())
            {
                await _ui.BuyCoins.Show();
            }
            
            return buy;
        }

        public async Task AddMovesAndBonuses()
        {
            _data.PlayerMoves = 5;
            _ui.Merge.UpdateMoves(_data.PlayerMoves);
            
            await Task.Yield();
        }

        public async Task<(int, int)> OnLevelWin(int levelReward)
        {
            Debug.Log("Level Done");
            await _ui.WellDone.Show();
            HideWellDone().DoAsync();

            var totalReward = await FinalClear(levelReward);
            var movesReward = totalReward - levelReward;
            
            await _ui.LevelComplete.Show(levelReward, movesReward);

            return (levelReward, movesReward);
        }

        private async Task HideWellDone()
        {
            await Task.Delay(300);
            await _ui.WellDone.Hide();
        }

        public async Task OnLevelFail()
        {
            await Task.Yield();
        }

        async Task WaitLevelComplete()
        {
            _levelComplete = false;
            while (!_levelComplete)
                await Task.Yield();
        }

    }
}