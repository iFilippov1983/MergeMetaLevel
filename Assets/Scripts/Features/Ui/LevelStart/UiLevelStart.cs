using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Components
{
    public class UiLevelStart : UiBaseView
    {
        public Button CloseBtn;
        public Button PlayBtn;
        public TextMeshProUGUI Caption;
        public Image TargetImg;
        
        public UiLevelStartApi Api;
    }
}