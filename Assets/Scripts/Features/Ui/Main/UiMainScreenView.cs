using Core;
using Components.Main;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Components
{
    public class UiMainScreenView : UiBaseView
    {
        public Button PlayBtn;
        public Button SettingsBtn;
        
        public ResourceView HeartsBtn;
        public ResourceView CoinsBtn;
        public ResourceView DiamondsBtn;
        public ResourceView XpBtn;
        
        public XpFly XpFly;
        public Transform StarsIcon;
        public Transform AlarmIcon;
        public UiMainScreenLevelsMove Levels;
        
        // Api
        public UiMainScreenApi Api;
    }
}