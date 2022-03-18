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

        _uiHandler.OnDiceRollClick += OnDiceRollClick;
        _metaLevel.OnResourcePickup += OnResourcePickupEvent;
        _metaLevel.OnFightEvent += OnFightEvent;
        _metaLevel.OnFightComplete += OnFightComplete;
    }

    private async void OnDiceRollClick()
    { 
        _uiHandler.DesactivateUiInteraction();

        int count = _metaLevel.GetRouteCellsCount();
        await _uiHandler.PlayDiceRollAnimation(count);
        await _metaLevel.MovePlayer();
        await _metaLevel.ApplyCellEvent();

        _uiHandler.ActivateUiInteraction();
    }

    private async void OnResourcePickupEvent(ResourceProperties resourceProperties)
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

    private async void OnFightEvent(EnemyProperties enemyProperties)
    {
        await _uiHandler.DisplayText("Fight!");
        //ui onFight functions
    }

    private async void OnFightComplete(bool playerWins)
    {
        string text;
        if (_playerProfile.Stats.LastFightWinner) text = "Victory!";
        else text = "Defeated!";
        await _uiHandler.DisplayText(text);
        //ui onFightComplete functions
    }

    private void OnDestroy()
    {
        _uiHandler.OnDiceRollClick -= OnDiceRollClick;
        _metaLevel.OnResourcePickup -= OnResourcePickupEvent;
        _metaLevel.OnFightEvent -= OnFightEvent;
        _metaLevel.OnFightComplete -= OnFightComplete;
    }
}

