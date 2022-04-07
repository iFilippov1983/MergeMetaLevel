using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace GameUI
{
    public class GameUIView : MonoBehaviour
    {
        [SerializeField] private ButtonView _rollButtonView;
        [SerializeField] private ButtonView _upgradePowerButtonView;
        [SerializeField] private ButtonView _playMergeButtonView;
        

        [SerializeField] private TextMeshProUGUI _upgradeCostTMP;
        [SerializeField] private TextMeshProUGUI _mergeLevelTMP;

        [SerializeField] private TextMeshProUGUI _goldTMP;
        [SerializeField] private TextMeshProUGUI _gemsTMP;
        [SerializeField] private TextMeshProUGUI _diceRollsTMP;
        [SerializeField] private TextMeshProUGUI _powerTMP;
        

        [SerializeField] private Text _mainText;

        public ButtonView RollButtonView => _rollButtonView;
        public ButtonView UpgradePowerButtonView => _upgradePowerButtonView;
        public ButtonView PlayMergeButtonView => _playMergeButtonView;
        public TextMeshProUGUI GoldTMP => _goldTMP;
        public TextMeshProUGUI GemsTMP => _gemsTMP;
        public TextMeshProUGUI DiceRollsTMP => _diceRollsTMP;
        public TextMeshProUGUI PowerTMP => _powerTMP;
        public TextMeshProUGUI UpgradeCostTMP => _upgradeCostTMP;
        public TextMeshProUGUI MergeLevelTMP => _mergeLevelTMP;
        public Text MainText => _mainText;
    }
}
