using Components.Main;
using TMPro;
using UnityEngine.UI;

namespace Components
{
    public class UiBuyItem : UiBaseView
    {
        public Button CloseBtn;
        public Button PlayOnBtn;
        public Button GiveUpBtn;
        public ResourceView Coins;
        public TextMeshProUGUI Cost;
        public TextMeshProUGUI Reward;
        
        public UiBuyItemApi Api;
    }
}