using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace GameUI
{
    public class GameUIView : MonoBehaviour
    {
        [SerializeField] private Button _rollButton;
        [SerializeField] private Button _upgradePowerButton;
        [SerializeField] private Button _playMergeButton;
        [SerializeField] private Text _mainText;
        [SerializeField] private Text _coinsText;
        [SerializeField] private Text _gemsText;
        [SerializeField] private Text _diceRollsText;
        [SerializeField] private Text _powerText;

        public Button RollButton => _rollButton;
        public Button UpgradePowerButton => _upgradePowerButton;
        public Button PlayMergeButton => _playMergeButton;
        public Text MainText => _mainText;
        public Text CoinsText => _coinsText;
        public Text GemsText => _gemsText;
        public Text DiceRollsText => _diceRollsText;
        public Text PowerText => _powerText;

        public void Init(UnityAction rollButtonClicked, UnityAction upgradePowerButtonClicked, UnityAction playMergeButtonClicked)
        {
            _rollButton.onClick.AddListener(rollButtonClicked);
            _upgradePowerButton.onClick.AddListener(upgradePowerButtonClicked);
            _playMergeButton.onClick.AddListener(playMergeButtonClicked);
        }

        private void OnDestroy()
        {
            _rollButton?.onClick.RemoveAllListeners();
            _upgradePowerButton?.onClick.RemoveAllListeners();
            _playMergeButton?.onClick.RemoveAllListeners();
        }
    }
}
