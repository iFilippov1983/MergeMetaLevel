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

        [SerializeField] private TextMeshProUGUI _upgradeCostTMP;
        [SerializeField] private TextMeshProUGUI _mergeLevelTMP;

        [SerializeField] private TextMeshProUGUI _goldTMP;
        [SerializeField] private TextMeshProUGUI _gemsTMP;
        [SerializeField] private TextMeshProUGUI _diceRollsTMP;
        [SerializeField] private TextMeshProUGUI _powerTMP;
        

        [SerializeField] private Text _mainText;

        public Button RollButton => _rollButton;
        public Button UpgradePowerButton => _upgradePowerButton;
        public Button PlayMergeButton => _playMergeButton;
        public TextMeshProUGUI GoldTMP => _goldTMP;
        public TextMeshProUGUI GemsTMP => _gemsTMP;
        public TextMeshProUGUI DiceRollsTMP => _diceRollsTMP;
        public TextMeshProUGUI PowerTMP => _powerTMP;
        public TextMeshProUGUI UpgradeCostTMP => _upgradeCostTMP;
        public TextMeshProUGUI MergeLevelTMP => _mergeLevelTMP;
        public Text MainText => _mainText;

        public void Init
            (
            UnityAction rollButtonClicked, 
            UnityAction upgradePowerButtonClicked, 
            UnityAction playMergeButtonClicked
            )
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
