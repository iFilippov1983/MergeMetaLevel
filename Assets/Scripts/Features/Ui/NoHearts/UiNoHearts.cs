using TMPro;
using UnityEngine.UI;

namespace Components
{
    public class UiNoHearts : UiBaseView
    {
        public Button CloseBtn;
        public Button GetFreeBtn;
        public TextMeshProUGUI Caption;
        public TextMeshProUGUI NextHeart;
        public TextMeshProUGUI Timer;
        
        public UiNoHeartsApi Api;
    }
}