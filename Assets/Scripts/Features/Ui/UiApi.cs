using System;
using Api.Ui;
using Core;
using Data;

namespace Components
{
    [Serializable]
    public class UiApi
    {
        public UiView View { get; private set; }

        public UiBlurApi Blur = new UiBlurApi();
        public UiLoadingApi Loading = new UiLoadingApi();
        public UiMainScreenApi MainScreen = new UiMainScreenApi();
        public UiMergeViewApi Merge = new UiMergeViewApi();
        public UiMergeToolViewApi MergeTool = new UiMergeToolViewApi();
        public UiBaseApi Stats = new UiBaseApi();
        public UiWellDoneApi WellDone = new UiWellDoneApi();
        public UiMergeSequenceApi MergeSequence = new UiMergeSequenceApi();
        public UiOutOfMovesApi OutOfMoves = new UiOutOfMovesApi();
        public UiBuyItemApi BuyCoins = new UiBuyItemApi();
        public UiNoStarsApi UiNoStars = new UiNoStarsApi();
        public UiLevelStartApi LevelStart = new UiLevelStartApi();
        public UiLevelCompleteNewApi LevelComplete = new UiLevelCompleteNewApi();
        public UiSettingsApi Settings = new UiSettingsApi();
        public UiTutorialApi Tutorial = new UiTutorialApi();
        public UiDebugApi Debug = new UiDebugApi();
        public UiNoHeartsApi NoHearts = new UiNoHeartsApi();

        public void SetCtx(UiView view, CoreRoot root)
        {
            View = view;

            View.Blur.Api = Blur;
            Blur.SetCtx(View.Blur);

            View.Loading.Api = Loading;
            Loading.SetCtx(View.Loading);

            View.MainScreen.Api = MainScreen;
            MainScreen.SetCtx(View.MainScreen, root);
            
            View.Merge.Api = Merge;
            Merge.SetCtx(View.Merge, root);
            
            View.MergeTool.Api = MergeTool;
            MergeTool.SetCtx(View.MergeTool, root);
            
            View.Stats.Api = Stats;
            Stats.SetCtxBase(View.Stats);
           
            View.WellDone.Api = WellDone;
            WellDone.SetCtx(View.WellDone, root);
            
            View.MergeSequence.Api = MergeSequence;
            MergeSequence.SetCtx(View.MergeSequence);
            
            View.OutOfMoves.Api = OutOfMoves;
            OutOfMoves.SetCtx(View.OutOfMoves, root);
            
            View.BuiCoins.Api = BuyCoins;
            BuyCoins.SetCtx(View.BuiCoins, root);
            
            View.LevelStart.Api = LevelStart;
            LevelStart.SetCtx(View.LevelStart, root);
           
            View.LevelComplete.Api = LevelComplete;
            LevelComplete.SetCtx(View.LevelComplete, root);
           
            View.UiNoStars.Api = UiNoStars;
            UiNoStars.SetCtx(View.UiNoStars, root);
           
            View.Settings.Api = Settings;
            Settings.SetCtx(View.Settings, root);
            
            Debug.SetCtx(View.UiDebug, root);
            View.UiDebug.Api = Debug;
            
            View.Tutorial.Api = Tutorial;
            Tutorial.SetCtx(View.Tutorial, View.TutorialElementsContainer, root.View.Merge.Player.Camera);
            
            NoHearts.SetCtx(View.NoHearts, root);
            View.NoHearts.Api = NoHearts;
        }


        public void OnBeforeMapEnter()
        {
            MainScreen.ShowImmediately();
        } 
        
        public void OnBeforeMapExit()
        {
            MainScreen.HideImmediately();
        }
        
        public void OnBeforeMergeEnter()
        {
            Merge.ShowImmediately();
        } 
        
        public void OnBeforeMergeExit()
        {
            Merge.HideImmediately();
            LevelComplete.HideImmediately();
        }

    }
}