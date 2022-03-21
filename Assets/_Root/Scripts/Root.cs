using Data;
using Player;
using GameUI;
using UnityEngine;
using UnityEngine.Analytics;
using System;
using System.Threading.Tasks;

internal sealed class Root : MonoBehaviour
{
    [SerializeField] private GameData _gameData;
    [SerializeField] private Transform _uiContainer;
    [SerializeField] private PlayerStats _initialPlayerStats;

    private PlayerProfile _playerProfile;
    private MetaLevel _metaLevel;
    private UIHandler _uiHandler;

    public GameData GameData => _gameData;

    private void Awake()
    {
        _playerProfile = new PlayerProfile(_initialPlayerStats);
        _metaLevel = new MetaLevel(_gameData, _playerProfile);
        _uiHandler = new UIHandler(_gameData.UIData, _uiContainer);
        _uiHandler.InitializeUI
            (
            _initialPlayerStats.Coins, 
            _initialPlayerStats.Gems, 
            _initialPlayerStats.DiceRolls,
            _initialPlayerStats.Power
            );

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
        await _uiHandler.PlayDiceUseAnimation();
        await _metaLevel.ApplyCellEvent();
        _uiHandler.ActivateUiInteraction();
    }

    private async Task MakeMove()
    {
        _uiHandler.DesactivateUiInteraction();

        int count = _metaLevel.GetRouteCellsCount();
        await _uiHandler.PlayDiceRollAnimation(count);
        await _metaLevel.MovePlayer();
        await _metaLevel.ApplyCellEvent();

        _uiHandler.ActivateUiInteraction();
    }

    private async void OnDiceRollsAmountChange(int amount)
    {
        await _uiHandler.ChangeRollsUI(amount);
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

        if (resouceType.Equals(ResouceType.Gems))
        {
            _playerProfile.Stats.Gems += resourceProperties.Amount;
            int amount = _playerProfile.Stats.Gems;
            await _uiHandler.ChangeGemsUI(amount);
        }
    }

    private async void OnFight(EnemyProperties enemyProperties)
    {
        await _uiHandler.DisplayText(UiString.Fight);
        //ui onFight functions
    }

    private async void OnFightComplete(bool playerWins)
    {
        string text;
        if (_playerProfile.Stats.LastFightWinner) text = UiString.Victory;
        else text = UiString.Defeated;
        await _uiHandler.DisplayText(text);
        //ui onFightComplete functions
    }

    private void SubscribeOnEvents()
    {
        _uiHandler.OnDiceRollClickEvent += OnDiceRollClick;
        _metaLevel.OnResourcePickupEvent += OnResourcePickup;
        _metaLevel.OnFightEvent += OnFight;
        _metaLevel.OnFightCompleteEvent += OnFightComplete;
        _metaLevel.OnDiceRollsAmontChadgeEvent += OnDiceRollsAmountChange;
    }

    private void OnDestroy()
    {
        _uiHandler.OnDiceRollClickEvent -= OnDiceRollClick;
        _metaLevel.OnResourcePickupEvent -= OnResourcePickup;
        _metaLevel.OnFightEvent -= OnFight;
        _metaLevel.OnFightCompleteEvent -= OnFightComplete;
        _metaLevel.Dispose();
    }
}

