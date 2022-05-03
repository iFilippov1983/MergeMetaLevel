using System.Globalization;
using Data;
using Game;
using GameUI;
using UnityEngine;
using UnityEngine.Analytics;
using System.Threading.Tasks;
using Components;
using Configs;
using Sirenix.OdinInspector;
using Utils;

internal sealed class Root : MonoBehaviour
{
    [SerializeField] private GameData _gameData;
    [SerializeField] private Transform _uiContainer;
    [SerializeField] private StaticData _configs;
    [SerializeField] private RootView _rootView;
    [SerializeField] private GameObject _level;
    [SerializeField] private GameObject _metaUi;
    [SerializeField] private PlayerStats _initialPlayerStats;
    [SerializeField] private bool _loadPlayerStatsFromFile;

    private ProgressHandler _progressHandler;
    private PlayerProfile _playerProfile;
    private MetaLevel _metaLevel;
    private UIHandler _uiHandler;
    [SerializeField, ReadOnly] private CoreRoot _coreRoot;

    private const int TargetFrameRate = 60;

    private void Awake()
    {
        TaskScheduler.UnobservedTaskException += HandleTaskException;
        Application.targetFrameRate = TargetFrameRate;
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

        _coreRoot = new CoreRoot(_rootView, _configs);
        _coreRoot.LoadProfile();
        _coreRoot.Run();

        var playerStats = _loadPlayerStatsFromFile
            ? _coreRoot.Data.Profile.PlayerStats
            : _initialPlayerStats;

        _playerProfile = new PlayerProfile(playerStats);
        _progressHandler = new ProgressHandler(_gameData.ProgressData, _playerProfile);
        _metaLevel = new MetaLevel(_gameData, _playerProfile);
        _uiHandler = new UIHandler(_gameData.UIData, _uiContainer, _playerProfile);

        Initialize();
    }
    
    private void Update()
    {
        _coreRoot.OnUpdate();
    }

    private void OnApplicationQuit()
    {
        _coreRoot.OnQuit();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        _coreRoot.OnPause(pauseStatus);
    }
    
    private void HandleTaskException(object sender, UnobservedTaskExceptionEventArgs e)
    {
        Debug.LogError(e.Exception);
    }

    private async void Initialize()
    {
        SubscribeOnEvents();
        await _uiHandler.ChangePowerUpgradeCostUi(_progressHandler.UpgradePrice.ToString());
        OnLevelCompletionProgress(_playerProfile.Stats.CurrentCellID);
    }

    private async void OnPlayMergeClicked()
    {      
        await _coreRoot.Ui.Loading.Show();//
        _metaLevel.HandlePlayerVisibility();
        _level.gameObject.SetActive(false);
        _metaUi.gameObject.SetActive(false);

        var (gameWin, goldReward) = await _coreRoot.PlayCore();

        await _coreRoot.Ui.Loading.Show();
        await _coreRoot.GoToMap(gameWin);
        _metaLevel.HandlePlayerVisibility();
        _level.gameObject.SetActive(true);
        _metaUi.gameObject.SetActive(true);

        await _coreRoot.Ui.Loading.Hide();//

        _progressHandler.HandleMergeLevelComplete(gameWin, goldReward);
        if (gameWin)
        {
            _uiHandler.ChangePowerUi(_playerProfile.Stats.Power.ToString()).DoAsync();
            _uiHandler.ChangeDiceRollsUi(_playerProfile.Stats.DiceRolls.ToString()).DoAsync();
            _uiHandler.ChangeGoldUi(_playerProfile.Stats.Gold);
            _uiHandler.ChangeMergeLevelButtonUi(_playerProfile.Stats.CurrentMergeLevel.ToString()).DoAsync();
            _uiHandler.ActivateUiInteraction(_progressHandler.CheckPlayerFunds());

            _metaLevel.AnimatePlayerLevelUp();
            await _uiHandler.PlayUpgradePowerAnimation(_progressHandler.GetPowerGain(gameWin));
        }
        else
        {
            _uiHandler.ActivateUiInteraction(_progressHandler.CheckPlayerFunds());
        }
        
        // _coreRoot.Events.Tutorial.Check.Invoke(TutorialTriggerType.MainScreen);
    }

    private async void OnUpgradePowerClicked()
    {
        bool canUpgrade = _progressHandler.CheckPlayerFunds();
        if (canUpgrade)
        {
            _uiHandler.DesactivateUiInteraction();

            _metaLevel.AnimatePlayerLevelUp();
            await _uiHandler.PlayUpgradePowerAnimation(_progressHandler.GetPowerGain());
            _progressHandler.MakePowerUpgrade();
            await _uiHandler.ChangePowerUi(_playerProfile.Stats.Power.ToString());
            _uiHandler.ChangeGoldUi(_playerProfile.Stats.Gold);
            await _uiHandler.ChangePowerUpgradeCostUi(_progressHandler.UpgradePrice.ToString());

            canUpgrade = _progressHandler.CheckPlayerFunds();
            _uiHandler.ActivateUiInteraction(canUpgrade);
            OnLevelCompletionProgress(_playerProfile.Stats.CurrentCellID);
        }
        else return;
    }

    private async void OnDiceRollClick()
    {
        _uiHandler.HideUI();

        bool haveRolls = _playerProfile.Stats.DiceRolls > 0;
        bool wasNotDefeated = _playerProfile.Stats.LastFightWinner;
        if (haveRolls && wasNotDefeated)
        {
            await MakeMove();
        }
        else if (haveRolls && !wasNotDefeated)
        {
            await RetryFight();
        }
        else if (!haveRolls)
        {
            await _uiHandler.DisplayText(UiString.NoMoreDiceRolls);
        };
    }

    //private async void OnResourcePickup(ResourceProperties resourceProperties)
    //{
    //    ResouceType resouceType = resourceProperties.ResouceType;

    //    if (resouceType.Equals(ResouceType.Gold))
    //    {
    //        _playerProfile.Stats.Gold += resourceProperties.Amount;
    //        await _uiHandler.ChangeGoldUi(_playerProfile.Stats.Gold);
    //    }

    //    if (resouceType.Equals(ResouceType.Power))
    //    {
    //        _playerProfile.Stats.Power += resourceProperties.Amount;
    //        int amount = _playerProfile.Stats.Power;
    //        await _uiHandler.ChangePowerUi(amount.ToString());
    //    }
    //}

    private async void OnResourcePickupExecute(ResourceProperties resourceProperties)
    {
        ResouceType resouceType = resourceProperties.ResouceType;

        if (resouceType.Equals(ResouceType.Gold))
        {
            return;
            //_playerProfile.Stats.Gold += resourceProperties.Amount;
            //await _uiHandler.ChangeGoldUi(_playerProfile.Stats.Gold);
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
        _uiHandler.ShowUI();
        string text = playerWins 
            ? UiString.Victory 
            : UiString.Defeated;
        await _uiHandler.DisplayText(text);
        await CheckLevelCompletion();
    }

    private async void OnResourcePickup()
    {
        _uiHandler.ShowUI();
        await CheckLevelCompletion();
    }

    private void OnLevelCompletionProgress(int currentCellId)
    {
        float progressValue = (float)currentCellId / (float)_gameData.LevelData.CellsViews.Length;
        _uiHandler.UpdateProgressBar(progressValue);
    }

    private async Task RetryFight()
    {
        _uiHandler.DesactivateUiInteraction();

        await _metaLevel.PrepareAction(false);
        await _metaLevel.ApplyCellEvent(OnFightComplete);

        _uiHandler.ActivateUiInteraction(_progressHandler.CheckPlayerFunds());
    }

    private async Task MakeMove()
    {
        _uiHandler.DesactivateUiInteraction();

        await _metaLevel.PrepareAction();
        await _uiHandler.ChangeDiceRollsUi(_playerProfile.Stats.DiceRolls.ToString());
        await _metaLevel.MakePlayerMove(OnFightComplete, OnResourcePickup);

        _uiHandler.ActivateUiInteraction(_progressHandler.CheckPlayerFunds());
    }

    private async Task CheckLevelCompletion()
    {
        var cellsViews = _gameData.LevelData.CellsViews;
        bool goNextLevel =
            _playerProfile.Stats.LastFightWinner == true
            &&
            _playerProfile.Stats.CurrentCellID.Equals(cellsViews.Length - 1);
        if (goNextLevel)
        {
            await _coreRoot.Ui.Loading.Show();
            _metaLevel.TeleportPlayerToStart();
            OnLevelCompletionProgress(_playerProfile.Stats.CurrentCellID);
            await _coreRoot.Ui.Loading.Hide();
        }
    }

    private void SubscribeOnEvents()
    {
        _uiHandler.OnDiceRollClickEvent += OnDiceRollClick;
        _uiHandler.OnUpgrdePowerClickEvent += OnUpgradePowerClicked;
        _uiHandler.OnPlayMergeButtonClicked += OnPlayMergeClicked;
        
        _metaLevel.OnFightEvent += OnFight;
        _metaLevel.OnPowerUpgradeAvailableEvent += OnPowerUpgradeAvailable;
        _metaLevel.OnLevelCompletionProgressEvent += OnLevelCompletionProgress;

        _metaLevel.OnGoldValueChangeEvent += _uiHandler.ChangeGoldUi;
    }

    private void OnDestroy()
    {
        _uiHandler.OnDiceRollClickEvent -= OnDiceRollClick;
        _uiHandler.OnUpgrdePowerClickEvent -= OnUpgradePowerClicked;
        _uiHandler.OnPlayMergeButtonClicked -= OnPlayMergeClicked;

        _metaLevel.OnFightEvent -= OnFight;
        _metaLevel.OnPowerUpgradeAvailableEvent -= OnPowerUpgradeAvailable;
        _metaLevel.OnLevelCompletionProgressEvent -= OnLevelCompletionProgress;
        _metaLevel.Cleanup();

        _metaLevel.OnGoldValueChangeEvent -= _uiHandler.ChangeGoldUi;

        TaskScheduler.UnobservedTaskException -= HandleTaskException;
    }
}

