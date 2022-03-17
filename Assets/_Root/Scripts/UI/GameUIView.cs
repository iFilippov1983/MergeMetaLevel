using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace GameUI
{
    public class GameUIView : MonoBehaviour
    {
        [SerializeField] private Button _rollButton;
        [SerializeField] private Text _numberText;
        [SerializeField] private Text _coinsText;
        [SerializeField] private Text _gemsText;

        public Button RollButton => _rollButton;
        public Text NumberText => _numberText;
        public Text CoinsText => _coinsText;
        public Text GemsText => _gemsText;

        public void Init(UnityAction rollButtonClicked)
        {
            _rollButton.onClick.AddListener(rollButtonClicked);
        }

        private void OnDestroy()
        {
            _rollButton.onClick.RemoveAllListeners();
        }
    }
}
