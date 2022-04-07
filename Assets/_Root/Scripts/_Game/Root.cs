using Data;
using Game;
using GameUI;
using UnityEngine;
using UnityEngine.Analytics;
using System.Threading.Tasks;

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
        //await Merge round play. Return win (true) or loose (false)
        bool levelComplete = true;

        _progressHandler.HandleMergeLevelComplete(levelComplete);
        if (levelComplete)
        {
            await _uiHandler.PlayUpgradePowerAnimation(_progressHandler.GetPowerGain(levelComplete));
            await _uiHandler.ChangePowerUi(_playerProfile.Stats.Power.ToString());
            await _uiHandler.ChangeDiceRollsUi(_playerProfile.Stats.DiceRolls.ToString());//temp
            await _uiHandler.ChangeMergeLevelButtonUi(_playerProfile.Stats.CurrentMergeLevel.ToString());
        }

        _uiHandler.ActivateUiInteraction(_progressHandler.CheckPlayerFunds());
    }

    private async void OnUpgradePowerClicked()
    {
        bool canUpgrade = _progressHandler.CheckPlayerFunds();
        if (canUpgrade)
        {
            _uiHandler.DesactivateUiInteraction();

            await _uiHandler.PlayUpgradePowerAnimation(_progressHandler.GetPowerGain());
            _progressHandler.MakePowerUpgrade();
            await _uiHandler.ChangePowerUi(_playerProfile.Stats.Power.ToString());
            await _uiHandler.ChangeGoldUi(_playerProfile.Stats.Gold.ToString());
            await _uiHandler.ChangePowerUpgradeCostUi(_progressHandler.UpgradePrice.ToString());

            canUpgrade = _progressHandler.CheckPlayerFunds();
            _uiHandler.ActivateUiInteraction(canUpgrade);
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
            await _uiHandler.ChangeGoldUi(amount.ToString());
        }

        if (resouceType.Equals(ResouceType.Power))
        {
            _playerProfile.Stats.Power += resourceProperties.Amount;
            int amount = _playerProfile.Stats.Power;
            await _uiHandler.ChangePowerUi(amount.ToString());
        }
    }

    private async void OnFight()
    {
        await _uiHandler.DisplayText(UiString.Fight);
        //TODO: ui onFight functions
    }

    private async void OnPowerUpgradeAvailable()
    {
        await _uiHandler.ChangePowerUpgradeCostUi(_progressHandler.UpgradePrice.ToString());
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

        _uiHandler.ActivateUiInteraction(_progressHandler.CheckPlayerFunds());
    }

    private async Task MakeMove()
    {
        _uiHandler.DesactivateUiInteraction();

        int count = _metaLevel.GetRouteCellsCount();
        await _uiHandler.PlayDiceRollAnimation(count);
        await _metaLevel.MovePlayer();
        await _metaLevel.ApplyCellEvent(OnFightComplete);

        _uiHandler.ActivateUiInteraction(_progressHandler.CheckPlayerFunds());
    }

    private void SubscribeOnEvents()
    {
        _uiHandler.OnDiceRollClickEvent += OnDiceRollClick;
        _uiHandler.OnUpgrdePowerClickEvent += OnUpgradePowerClicked;
        _uiHandler.OnPlayMergeButtonClicked += OnPlayMergeClicked;

        _metaLevel.OnResourcePickupEvent += OnResourcePickup;
        _metaLevel.OnFightEvent += OnFight;
        _metaLevel.OnPowerUpgradeAvailableEvent += OnPowerUpgradeAvailable;
    }

    private void OnDestroy()
    {
        _uiHandler.OnDiceRollClickEvent -= OnDiceRollClick;
        _uiHandler.OnUpgrdePowerClickEvent -= OnUpgradePowerClicked;
        _uiHandler.OnPlayMergeButtonClicked -= OnPlayMergeClicked;

        _metaLevel.OnResourcePickupEvent -= OnResourcePickup;
        _metaLevel.OnFightEvent -= OnFight;
        _metaLevel.OnPowerUpgradeAvailableEvent -= OnPowerUpgradeAvailable;
    }
}

