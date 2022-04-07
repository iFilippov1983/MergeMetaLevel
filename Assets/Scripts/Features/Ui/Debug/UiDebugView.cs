using TMPro;
using UnityEngine.UI;

namespace Components
{
    public class UiDebugView : UiBaseView
    {
        public Button Add5Xp;
        public Button Add100Coins;
        public Button ClearProfile;
        public Toggle SkipQuests;
        public Toggle SkipTutors;
        public Toggle SkipMerge;
        public TMP_InputField Level;
        public Button Close;
        
        public UiDebugApi Api;
    }
}