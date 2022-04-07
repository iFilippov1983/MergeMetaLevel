using Core;
using Components.Ui.Map;
using UnityEngine;

namespace Components
{
    public class UiView : MonoBehaviour
    {
        public UiBlurView Blur;
        public UiStatsPanelView Stats;
        public UiLoadingView Loading;
        public UiMainScreenView MainScreen;
        public UiMergeView Merge;
        public UiMergeToolView MergeTool;
        public UiMergeSequenceView MergeSequence;
        public UiOutOfMoves OutOfMoves;
        public UiBuyItem BuiCoins;
        public XpFly XpFly;
        public UiLevelCompleteView LevelComplete;
        public UiSettingsView Settings;
        public UiNoHearts NoHearts;

        // public UiApi Api;
        public UiHoldProgress UiHoldProgress;
        public UiLevelStart LevelStart;
        
        public UiNoStars UiNoStars;
        public UiWellDoneView WellDone;
        public UiDebugView UiDebug;
        
        public UiTutorialView Tutorial;
        public RectTransform TutorialElementsContainer;
    }
}