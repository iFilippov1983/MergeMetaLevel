using System;
using System.Threading.Tasks;
using Components;
using Components.Api.Tutorial;
using Components.Services;
using Components.Systems;
using Configs;
using Configs.Tutorial;
using Data;
using Features._Events;
using UnityEngine;
using Utils;

[Serializable]
public class CoreRoot
{
    public static CoreRoot tmpInstance;
    
    public RootView View;
    public StaticData Configs;
    public DynamicData Data = new DynamicData();
    public RootEvents Events = new RootEvents();
    
    public SaveApi Save = new SaveApi();
    public HeartsApi Hearts = new HeartsApi();
    public TimeApi Time = new TimeApi();
    public MergeApi Merge = new MergeApi();
    public UiApi Ui = new UiApi();
    public CheatsApi Cheats = new CheatsApi();
    public TutorialApi Tutorial = new TutorialApi();

    public CoreRoot(RootView rootView, StaticData configs)
    {
        tmpInstance = this;
        
        View = rootView;
        Configs = configs;
    }
    
    public void Run()
    {
        SetCtx();
        Subscribe();
        
        NotifyProfileReady();
        SetPreviewData();
        
        Merge.SetLevelIfNull(Data.Profile.LevelIndex);
        PrepareMainScreen();

        Time.Run();
        Events.Tutorial.Check.Invoke(TutorialTriggerType.MainScreen);
    }

    public void LoadProfile()
    {
        Save.LoadProfile(Data);
    }


    public void OnQuit()
    {
        SaveProfile();
    }

    public void OnPause(bool pause)
    {
        Events.App.OnAppPause?.Invoke(pause);
    }


    public void OnUpdate()
    {
        Merge.OnUpdate();
        Events.App.OnUpdate?.Invoke();
    }

    private void SetCtx()
    {
        Data.SetCtx(Events);
        View.SetCtx();
        Configs.SetCtx(this);
        Save.SetCtx(this);
        Time.SetCtx(this);

        Hearts.SetCtx(this);
        Cheats.SetCtx(this);
        Ui.SetCtx(View.Ui, this);
        Merge.SetCtx(View.Merge, Data.Merge, Events.Merge, Events.Tutorial, Configs.Merge,Configs.Shop, Data.Profile, Ui, this);
        Tutorial.SetCtx(this);
    }

    private void NotifyProfileReady()
    {
        Save.NotifyProfileReady();
    }

    private void SetPreviewData()
    {
        View.DataPreview = Data;
    }

    private void SaveProfile()
    {
        Save.SaveProfile();
    }

    
    private async Task<(bool, int)> GoToMerge()
    {
        // await Ui.Loading.Show();

        var (gameWin, coinsReward, movesReward) = (false, 0, 0);
        gameWin = Configs.Debug.SkipMerge;
        if (!Configs.Debug.SkipMerge)
        {
            PrepareMergeScreen();
            TinySauce.OnGameStarted($"{Data.Profile.LevelIndex + 1}" );
            
            Merge.PreparePlayerLevel();
            await Task.Delay(200);
            await Ui.Loading.Hide();
            await Ui.Merge.WaitGoComplete();
            (gameWin, coinsReward, movesReward) = await PlayLevel();

            TinySauce.OnGameFinished(gameWin, coinsReward, $"{Data.Profile.LevelIndex + 1}" );
        }


        if (gameWin)
        {
            Data.Profile.Coins += (coinsReward + movesReward) ;
            Data.Profile.Xp++;
            Data.Profile.LevelIndex++;
            SetLevelByIndex();
        }
        Save.SaveProfile();

        int reward = coinsReward + movesReward;
        return (gameWin, reward);
        
        // await GoToMap(gameWin);
    }

    // public async Task OpenLevelStartFromMainMenu()
    // {
    //     Data.Ui.PlayFromQuests = false;
    //     await OpenLevelStart();
    // }
    //
    // private async void OpenLevelStartFromQuests()
    // {
    //     Data.Ui.PlayFromQuests = true;
    //     await OpenLevelStart();
    // }
    //
    // private async Task OpenLevelStart()
    // {
    //     var ok = await CheckHearts();
    //     if (!ok)
    //         return;
    //         
    //     Ui.MainScreen.DoHide().DoAsync();
    //     
    //     await PlayCore();
    //
    //     // SetLevelByIndex();
    //     // Ui.MainScreen.DoHide().DoAsync();
    //     // var doStart = await Ui.LevelStart.Show();
    //     // if (doStart)
    //     //     GoToMerge();
    //     // else
    //     //     await Ui.MainScreen.DoShow();
    // }

    public async Task<(bool, int)> PlayCore()
    {
        SetLevelByIndex();
        var (gameWin, reward) = await GoToMerge();
        if (!gameWin)
            Data.Profile.Hearts--;
        return (gameWin, reward);
    }

    public async Task<bool> CheckHearts()
    {
        if (Data.Profile.Hearts == 0)
        {
            var ads = await Ui.NoHearts.Show();
            if (!ads)
                return false;
        }

        return true;
    }

    private void SetLevelByIndex()
    {
        Data.Merge.Level = Configs.Merge.Levels.SaveGet(Data.Profile.LevelIndex);
        
        if (Data.Merge.Level == null) // out of bounds
        {
            Data.Profile.LevelIndex = 0;
            Data.Merge.Level = Configs.Merge.Levels.SaveGet(Data.Profile.LevelIndex);
        }
    }

    private async Task UseMergeTool(string name)
    {
        var use = await Ui.MergeTool.Show(name);
        if(use)
            Events.Merge.UseTool?.Invoke(name);
    }

    private async void GoToMapFromPauseMenu()
        => await GoToMap(false);
    
    public async Task GoToMap(bool gameWin)
    {
        // await Ui.Loading.Show();
        
        PrepareMainScreen();

        // await Ui.Loading.Hide();
        
        // if(gameWin)
            // await Ui.MainScreen.MoveLevels();

        // Events.Tutorial.Check.Invoke(TutorialTriggerType.MainScreen);
    }

    private async Task<(bool, int, int)> PlayLevel()
    {
        Debug.Log("Play level");
        if (Data.Merge.GameStart)
            throw new Exception("PlayLevel twice");

        // DynamicData.Merge.Input.InputLocked = true;
        Data.Merge.GameStart = true;
        Data.Merge.GameWin = false;
        
        var gameWin = await Merge.PlayLevel();
        
        Data.Merge.Input.InputLocked = true;
        Data.Merge.Input.SkipPressed = false;
        
        Data.Merge.GameStart = false;
        Data.Merge.GameWin = gameWin;
        
        var (levelReward, movesReward) = (50, 0);
        
        if (gameWin)
            (levelReward, movesReward) = await Merge.OnLevelWin(levelReward);
        else
            await Merge.OnLevelFail();

        return (gameWin, levelReward, movesReward);
    }

    private void PrepareMainScreen()
    {
        Ui.OnBeforeMergeExit();
        // Ui.OnBeforeMapEnter();
        
        Merge.SetInactive();
    }

    private void PrepareMergeScreen()
    {
        // Ui.OnBeforeMapExit();
        Ui.OnBeforeMergeEnter();
        Merge.SetActive();
    }
    
    private void Subscribe()
    {
        // Ui.MainScreen.OnPlayBtnClick += OpenLevelStartFromMainMenu;
        Ui.MainScreen.OnSettingsBtnClick += OpenSettings;
        
        Ui.Merge.OnPauseBtnClick += () => GoToMapFromPauseMenu();
        Ui.Merge.OnToolClick += async (name) => await UseMergeTool(name);
    }

    private async void OpenSettings()
    {
        Ui.MainScreen.DoHide().DoAsync();
        
    // #if UNITY_EDITOR
        await Ui.Debug.Show();
    // #else
        // await Ui.Settings.Show();
    // #endif
        
        await Ui.MainScreen.DoShow();
    }
}