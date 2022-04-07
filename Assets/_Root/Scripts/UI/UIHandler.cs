using System;
using System.Threading.Tasks;
using UnityEngine;
using Data;
using Object = UnityEngine.Object;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

namespace GameUI
{
    internal class UIHandler
    {
        private Transform _uiContainer;
        private UIData _uiData;
        private GameUIView _gameUIView;
        private PlayerProfile _playerProfile;

        public Action OnDiceRollClickEvent;
        public Action OnUpgrdePowerClickEvent;
        public Action OnPlayMergeButtonClicked;

        public UIHandler(UIData uiData, Transform uiContainer, PlayerProfile playerProfile)
        {
            _uiContainer = uiContainer;
            _uiData = uiData;
            _playerProfile = playerProfile;

            InitializeUI();
        }

        public async Task PlayDiceRollAnimation(int number = 0) //when time to move
        {
            _gameUIView.MainText.text = number.ToString();
            _gameUIView.MainText.gameObject.SetActive(true);
            await Task.Delay(1000);
            _gameUIView.MainText.text = string.Empty;
            _gameUIView.MainText.gameObject.SetActive(false);
            --_playerProfile.Stats.DiceRolls;
            await ChangeDiceRollsUi(_playerProfile.Stats.DiceRolls.ToString());
        }

        public async Task PlayDiceUseAnimation() //when next fight attempt
        {
            _gameUIView.MainText.text = UiString.NextAttempt;
            _gameUIView.MainText.gameObject.SetActive(true);
            await Task.Delay(1000);
            _gameUIView.MainText.text = string.Empty;
            _gameUIView.MainText.gameObject.SetActive(false);
            --_playerProfile.Stats.DiceRolls;
            await ChangeDiceRollsUi(_playerProfile.Stats.DiceRolls.ToString());
        }

        public async Task PlayUpgradePowerAnimation(int powerAmountToShow)
        {
            string text = string.Concat("+", powerAmountToShow);
            _gameUIView.MainText.text = text;
            _gameUIView.MainText.gameObject.SetActive(true);
            await Task.Delay(1000);
            _gameUIView.MainText.text = string.Empty;
            _gameUIView.MainText.gameObject.SetActive(false);
        }

        public async Task PlayGoToMergeAnimation()
        {
            _gameUIView.MainText.text = UiString.PlayingMerge;
            _gameUIView.MainText.gameObject.SetActive(true);
            await Task.Delay(1000);
            _gameUIView.MainText.text = string.Empty;
            _gameUIView.MainText.gameObject.SetActive(false);

        }

        public async Task DisplayText(string text)
        {
            _gameUIView.MainText.text = text;
            _gameUIView.MainText.gameObject.SetActive(true);
            await Task.Delay(1000);
            _gameUIView.MainText.text = string.Empty;
            _gameUIView.MainText.gameObject.SetActive(false);
        }

        public void ActivateUiInteraction()
        {
            ButtonView rollButton = _gameUIView.RollButtonView;
            rollButton.Button.interactable = true;
            SetButtonImageColor(rollButton.ImageList, rollButton.DefaultColor);
            SetButtonTextAlfa(rollButton.TextList);

            ButtonView mergeButton = _gameUIView.PlayMergeButtonView;
            mergeButton.Button.interactable = true;
            SetButtonImageColor(mergeButton.ImageList, mergeButton.DefaultColor);
            SetButtonTextAlfa(mergeButton.TextList);

            ButtonView upgradeButton = _gameUIView.UpgradePowerButtonView;
            upgradeButton.Button.gameObject.SetActive(_playerProfile.Stats.PowerUpgradeAvailable);
            upgradeButton.Button.interactable = true;
            SetButtonImageColor(upgradeButton.ImageList, upgradeButton.DefaultColor);
            SetButtonTextAlfa(upgradeButton.TextList);
        }

        public void DesactivateUiInteraction()
        {
            ButtonView rollButton = _gameUIView.RollButtonView;
            rollButton.Button.interactable = false;
            SetButtonImageColor(rollButton.ImageList, rollButton.FadeColor);
            SetButtonTextAlfa(rollButton.TextList, true);

            ButtonView mergeButton = _gameUIView.PlayMergeButtonView;
            mergeButton.Button.interactable = false;
            SetButtonImageColor(mergeButton.ImageList, mergeButton.FadeColor);
            SetButtonTextAlfa(mergeButton.TextList, true);

            ButtonView upgradeButton = _gameUIView.UpgradePowerButtonView;
            upgradeButton.Button.interactable = false;
            SetButtonImageColor(upgradeButton.ImageList, upgradeButton.FadeColor);
            SetButtonTextAlfa(upgradeButton.TextList, true);
        }

        public async Task ChangeGoldUi(string amount)
        { 
            await Task.Delay(100);//coins fly animation
            _gameUIView.GoldTMP.text = amount;
        }

        public async Task ChangeGemsUI(int amount)
        {
            await Task.Delay(100);//gems fly animation
            _gameUIView.GemsTMP.text = amount.ToString();
        }

        internal async Task ChangePowerUi(string amount)
        {
            await Task.Delay(100);//power change animation
            _gameUIView.PowerTMP.text = amount;
        }

        internal async Task ChangeDiceRollsUi(string amount)
        {
            await Task.Delay(100);//dice change animation
            _gameUIView.DiceRollsTMP.text = amount;
        }

        internal async Task ChangePowerUpgradeCostUi(string cost)
        {
            await Task.Delay(100);//cost change animation
            _gameUIView.UpgradeCostTMP.text = cost;
        }

        internal async Task ChangeMergeLevelButtonUi(string level)
        {
            await Task.Delay(100);
            _gameUIView.MergeLevelTMP.text = level;
        }

        private void InitializeUI()
        {
            PlayerStats stats = _playerProfile.Stats;
            var uiObject = Object.Instantiate(_uiData.GameUIPrefab, _uiContainer);
            _gameUIView = uiObject.GetComponent<GameUIView>();
            _gameUIView.RollButtonView.Button.onClick.AddListener(DiceRoll);
            _gameUIView.UpgradePowerButtonView.Button.onClick.AddListener(UpgradePower);
            _gameUIView.PlayMergeButtonView.Button.onClick.AddListener(PlayMerge);

            _gameUIView.GoldTMP.text = stats.Gold.ToString();
            _gameUIView.GemsTMP.text = stats.Gems.ToString();
            _gameUIView.DiceRollsTMP.text = stats.DiceRolls.ToString();
            _gameUIView.PowerTMP.text = stats.Power.ToString();
            _gameUIView.MergeLevelTMP.text = stats.CurrentMergeLevel.ToString();

            _gameUIView.UpgradePowerButtonView.gameObject.SetActive(stats.PowerUpgradeAvailable);
        }

        private void SetButtonTextAlfa(List<TextMeshProUGUI> textList, bool fade = false)
        {
            if (textList == null) return;
            float halfFade = 0.5f;
            float fullAlfa = 1f;

            foreach (var tmp in textList)
                tmp.alpha = fade ? halfFade : fullAlfa;
        }

        private void SetButtonImageColor(List<Image> imageList, Color color)
        {
            if (imageList == null) return;
            foreach (var img in imageList)
                img.color = color;
        }

        private void DiceRoll()
        { 
            OnDiceRollClickEvent?.Invoke();
        }

        private void UpgradePower()
        {
            OnUpgrdePowerClickEvent?.Invoke();
        }

        private void PlayMerge()
        {
            OnPlayMergeButtonClicked?.Invoke();
        }
    }
}
