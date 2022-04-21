using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace GameUI
{
    public class GameUIView : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Dice2D _dice;
        [SerializeField] private ProgressBarView _progressBarView;
        [SerializeField] private ButtonView _rollButtonView;
        [SerializeField] private ButtonView _upgradePowerButtonView;
        [SerializeField] private ButtonView _playMergeButtonView;

        [SerializeField] private Image _rollsAmountBeacon;

        [SerializeField] private TextMeshProUGUI _upgradeCostTMP;
        [SerializeField] private TextMeshProUGUI _mergeLevelTMP;

        [SerializeField] private TextMeshProUGUI _goldTMP;
        [SerializeField] private TextMeshProUGUI _gemsTMP;
        [SerializeField] private TextMeshProUGUI _diceRollsTMP;
        [SerializeField] private TextMeshProUGUI _powerTMP;

        [SerializeField] private TextMeshProUGUI _mainTMP;
        [SerializeField] private GameObject _extraPowerImage;

        public Animator Animator => _animator;
        public Dice2D Dice => _dice;
        public ProgressBarView ProgressBarView => _progressBarView;
        public ButtonView RollButtonView => _rollButtonView;
        public ButtonView UpgradePowerButtonView => _upgradePowerButtonView;
        public ButtonView PlayMergeButtonView => _playMergeButtonView;
        public Image RollsAmountBeacon => _rollsAmountBeacon;
        public TextMeshProUGUI GoldTMP => _goldTMP;
        public TextMeshProUGUI GemsTMP => _gemsTMP;
        public TextMeshProUGUI DiceRollsTMP => _diceRollsTMP;
        public TextMeshProUGUI PowerTMP => _powerTMP;
        public TextMeshProUGUI UpgradeCostTMP => _upgradeCostTMP;
        public TextMeshProUGUI MergeLevelTMP => _mergeLevelTMP;
        public TextMeshProUGUI MainTMP => _mainTMP;
        public GameObject ExtraPowerImage => _extraPowerImage;
    }
}
