using Components.Main;
using TMPro;
using UnityEngine.UI;

namespace Components
{
    public class UiOutOfMoves : UiBaseView
    {
        public Button CloseBtn;
        public Button PlayOnBtn;
        public Button GiveUpBtn;
        public ResourceView Coins;
        public TextMeshProUGUI Cost;
        
        public UiOutOfMovesApi Api;
    }
}