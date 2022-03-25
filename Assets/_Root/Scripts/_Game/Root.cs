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

    private PlayerProfile _playerProfile;
    private MetaLevel _metaLevel;
    private UIHandler _uiHandler;

    private Action<int> OnDiceRollsAmountChangedEvent;

    private void Awake()
    {
        _playerProfile = new PlayerProfile(_initialPlayerStats);
        _metaLevel = new MetaLevel(_gameData, _playerProfile);
        _uiHandler = new UIHandler(_gameData.UIData, _uiContainer);
        _uiHandler.InitializeUI(_initialPlayerStats);

        SubscribeOnEvents();
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

    private async Task RetryFight()
    {
        _uiHandler.DesactivateUiInteraction();
        await _uiHandler.PlayDiceUseAnimation(OnDiceRollsAmountChangedEvent);
        await _metaLevel.ApplyCellEvent(OnFightComplete);//callback
        _uiHandler.ActivateUiInteraction();
    }

    private async Task MakeMove()
    {
        _uiHandler.DesactivateUiInteraction();

        int count = _metaLevel.GetRouteCellsCount();
        await _uiHandler.PlayDiceRollAnimation(count, OnDiceRollsAmountChange); // OnDiceRollsAmountChangedEvent);
        await _metaLevel.MovePlayer();
        await _metaLevel.ApplyCellEvent(OnFightComplete);//callback

        _uiHandler.ActivateUiInteraction();
    }

    private async void OnDiceRollsAmountChange(int amount)
    {
        _playerProfile.Stats.DiceRolls += amount;
        await _uiHandler.ChangeRollsUI(_playerProfile.Stats.DiceRolls);
    }

    private async void OnResourcePickup(ResourceProperties resourceProperties)
    {
        ResouceType resouceType = resourceProperties.ResouceType;

        if (resouceType.Equals(ResouceType.Coins))
        {
            _playerProfile.Stats.Coins += resourceProperties.Amount;
            int amount = _playerProfile.Stats.Coins;
            await _uiHandler.ChangeCoinsUI(amount);
        }

        if (resouceType.Equals(ResouceType.Power))
        {
            _playerProfile.Stats.Power += resourceProperties.Amount;
            int amount = _playerProfile.Stats.Power;
            await _uiHandler.ChangePowerUi(amount);
        }
    }

    private async void OnFight()//(EnemyProperties enemyProperties)
    {
        await _uiHandler.DisplayText(UiString.Fight);
        //ui onFight functions
    }

    private async void OnFightComplete(bool playerWins)
    {
        string text = _playerProfile.Stats.LastFightWinner 
            ? UiString.Victory 
            : UiString.Defeated;
        await _uiHandler.DisplayText(text);
        //ui onFightComplete functions
    }

    private void SubscribeOnEvents()
    {
        OnDiceRollsAmountChangedEvent += OnDiceRollsAmountChange;

        _uiHandler.OnDiceRollClickEvent += OnDiceRollClick;

        _metaLevel.OnResourcePickupEvent += OnResourcePickup;
        _metaLevel.OnFightEvent += OnFight;
    }

    private void OnDestroy()
    {
        OnDiceRollsAmountChangedEvent -= OnDiceRollsAmountChange;

        _uiHandler.OnDiceRollClickEvent -= OnDiceRollClick;

        _metaLevel.OnResourcePickupEvent -= OnResourcePickup;
        _metaLevel.OnFightEvent -= OnFight;

        _metaLevel.Dispose();
    }
}

