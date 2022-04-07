using System;
using System.Collections.Generic;
using Core;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Components
{
    [Serializable]
    public class ToolButton
    {
        public string name;
        public Button button;
    }
    
    public class UiMergeView : UiBaseView
    {
        public Transform XpFrom;
        public Transform XpTo;
        public Transform UiGo;
        public Button BackBtn;
        public Button QuestsBtn;
        public Transform MovesPos;
        public TextMeshProUGUI MovesTxt;
        public TextMeshProUGUI LevelTxt;
        // public CanvasGroup WellDone;
        public Image GoalImg;
        public Transform EditorActive;
        [TableList] 
        public List<ToolButton> ToolButtons;
        public Button Cheat_LevelFailBtn;
        public Button Cheat_LevelWinBtn;

        public Image CoinsImage;
        public Transform CoinsFlyCenter;
        public CanvasGroup CoinsGroup;
        public TextMeshProUGUI CoinsText;
        public AnimatorEvent GoReadyEvent;

        // Api
        public UiMergeViewApi Api;
    }
}