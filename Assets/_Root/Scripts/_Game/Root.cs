using Data;
using Game;
using GameUI;
using UnityEngine;
using UnityEngine.Analytics;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

internal sealed class Root : MonoBehaviour
{
    [SerializeField] private GameData _gameData;
    [SerializeField] private Transform _uiContainer;
    [SerializeField] private PlayerStats _initialPlayerStats;

    private ProgressHandler _progressHandler;
    private PlayerProfile _playerProfile;
    private MetaLevel _metaLevel;
    private UIHandler _uiHandler;

    private void Awake()
    {
        _playerProfile = new PlayerProfile(_initialPlayerStats);
        _progressHandler = new ProgressHandler(_gameData.ProgressData, _playerProfile);
        _metaLevel = new MetaLevel(_gameData, _playerProfile);
        _uiHandler = new UIHandler(_gameData.UIData, _uiContainer, _playerProfile);

        SubscribeOnEvents();
    }

    private async void OnPlayMergeClicked()
    {
        _uiHandler.DesactivateUiInteraction();
        await _uiHandler.PlayGoToMergeAnimation();
        _uiHandler.ActivateUiInteraction();
    }

    private async void OnUpgradePowerClicked()
    {
        bool canBuy = _progressHandler.CheckPlayerFunds();
        if (canBuy)
        {
            _uiHandler.DesactivateUiInteraction();
            await _uiHandler.PlayUpgradePowerAnimation(_progressHandler.PowerGain);
            _progressHandler.MakePowerUpgrade();
            await _uiHandler.ChangePowerUi(_playerProfile.Stats.Power);
            _uiHandler.ActivateUiInteraction();
        }
        else return;
    }

    private async void OnDiceRollClick()
    {
        bool haveRolls = _playerProfile.Stats.DiceRolls > 0;
        bool wasNotDefated = _playerProfile.Stats.LastFightWinner;
        if (haveRolls && wasNotDefated)
        {
            await MakeMove();
        }
        else if (haveRolls && !wasNotDefated)
        {
            await RetryFight();
        }
        else if (!haveRolls)
        {
            await _uiHandler.DisplayText(UiString.NoMoreDiceRolls);
        }
    }

    private async void OnResourcePickup(ResourceProperties resourceProperties)
    {
        ResouceType resouceType = resourceProperties.ResouceType;

        if (resouceType.Equals(ResouceType.Gold))
        {
            _playerProfile.Stats.Gold += resourceProperties.Amount;
            int amount = _playerProfile.Stats.Gold;
            await _uiHandler.ChangeCoinsUi(amount);
        }

        if (resouceType.Equals(ResouceType.Power))
        {
            _playerProfile.Stats.Power += resourceProperties.Amount;
            int amount = _playerProfile.Stats.Power;
            await _uiHandler.ChangePowerUi(amount);
        }
    }

    private async void OnFight()
    {
        await _uiHandler.DisplayText(UiString.Fight);
        //TODO: ui onFight functions
    }

    private async void OnFightComplete(bool playerWins)
    {
        string text = _playerProfile.Stats.LastFightWinner 
            ? UiString.Victory 
            : UiString.Defeated;
        await _uiHandler.DisplayText(text);
        //TODO: ui onFightComplete functions
    }

    private async Task RetryFight()
    {
        _uiHandler.DesactivateUiInteraction();
        await _uiHandler.PlayDiceUseAnimation();
        await _metaLevel.ApplyCellEvent(OnFightComplete);
        _uiHandler.ActivateUiInteraction();
    }

    private async Task MakeMove()
    {
        _uiHandler.DesactivateUiInteraction();

        int count = _metaLevel.GetRouteCellsCount();
        await _uiHandler.PlayDiceRollAnimation(count);
        await _metaLevel.MovePlayer();
        await _metaLevel.ApplyCellEvent(OnFightComplete);

        _uiHandler.ActivateUiInteraction();
    }

    private void SubscribeOnEvents()
    {
        _uiHandler.OnDiceRollClickEvent += OnDiceRollClick;
        _uiHandler.OnUpgrdePowerClickEvent += OnUpgradePowerClicked;
        _uiHandler.OnPlayMergeButtonClicked += OnPlayMergeClicked;

        _metaLevel.OnResourcePickupEvent += OnResourcePickup;
        _metaLevel.OnFightEvent += OnFight;
    }

    private void OnDestroy()
    {
        _uiHandler.OnDiceRollClickEvent -= OnDiceRollClick;
        _uiHandler.OnUpgrdePowerClickEvent -= OnUpgradePowerClicked;
        _uiHandler.OnPlayMergeButtonClicked -= OnPlayMergeClicked;

        _metaLevel.OnResourcePickupEvent -= OnResourcePickup;
        _metaLevel.OnFightEvent -= OnFight;
    }
}

