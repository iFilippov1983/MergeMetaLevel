using System;
using System.Threading.Tasks;
using UnityEngine;
using Data;
using Object = UnityEngine.Object;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using Lofelt.NiceVibrations;
using static UnityEngine.ParticleSystem;
using Unity.Collections;

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

        public async Task PlayUpgradePowerAnimation(int powerAmountToShow)
        {
            string text = string.Concat("+", powerAmountToShow);
            var popup = _gameUIView.UpgradePowerButtonView.ButtonText;
            popup.text = text;
            popup.gameObject.SetActive(true);
            await Task.Delay(2000);
            popup.text = string.Empty;
            popup.gameObject.SetActive(false);
        }

        public async Task DisplayText(string text)
        {
            _gameUIView.MainTMP.text = text;
            _gameUIView.MainTMP.gameObject.SetActive(true);
            await Task.Delay(2000);
            _gameUIView.MainTMP.text = string.Empty;
            _gameUIView.MainTMP.gameObject.SetActive(false);
        }

        public void HideUI()
        {
            _gameUIView.Animator.SetTrigger(UiString.Hide);
        }

        public void ShowUI()
        {
            _gameUIView.Animator.SetTrigger(UiString.Show);
        }

        public void ActivateUiInteraction(bool upgradeButtonActive)
        {          
            ButtonView rollButton = _gameUIView.RollButtonView;
            rollButton.Button.interactable = true;
            SetButtonImageColor(rollButton.ImageList, rollButton.DefaultColor);
            SetButtonTextAlfa(rollButton.TextList);
            rollButton.Button.image.sprite = 
                _playerProfile.Stats.DiceRolls > 0 
                ? rollButton.DefaultSprite 
                : rollButton.UnactiveStateSprite;
            _gameUIView.RollsAmountBeacon.gameObject.SetActive(_playerProfile.Stats.DiceRolls > 0);
            SetRollButton(_playerProfile.Stats.LastFightWinner);

            ButtonView mergeButton = _gameUIView.PlayMergeButtonView;
            mergeButton.Button.interactable = true;
            SetButtonImageColor(mergeButton.ImageList, mergeButton.DefaultColor);
            SetButtonTextAlfa(mergeButton.TextList);

            ButtonView upgradeButton = _gameUIView.UpgradePowerButtonView;
            upgradeButton.Button.gameObject.SetActive(_playerProfile.Stats.PowerUpgradeAvailable);
            upgradeButton.Button.interactable = true;
            SetButtonImageColor(upgradeButton.ImageList, upgradeButton.DefaultColor);
            SetButtonTextAlfa(upgradeButton.TextList);
            upgradeButton.Button.image.sprite =
                upgradeButtonActive
                ? upgradeButton.DefaultSprite
                : upgradeButton.UnactiveStateSprite;
        }

        public void DesactivateUiInteraction()
        {
            ButtonView rollButton = _gameUIView.RollButtonView;
            rollButton.Button.interactable = false;
            SetButtonImageColor(rollButton.ImageList, rollButton.FadeColor);
            SetButtonTextAlfa(rollButton.TextList, true);
            _gameUIView.RollsAmountBeacon.gameObject.SetActive(false);
            SetRollButton(_playerProfile.Stats.LastFightWinner);

            ButtonView mergeButton = _gameUIView.PlayMergeButtonView;
            mergeButton.Button.interactable = false;
            SetButtonImageColor(mergeButton.ImageList, mergeButton.FadeColor);
            SetButtonTextAlfa(mergeButton.TextList, true);

            ButtonView upgradeButton = _gameUIView.UpgradePowerButtonView;
            upgradeButton.Button.interactable = false;
            SetButtonImageColor(upgradeButton.ImageList, upgradeButton.FadeColor);
            SetButtonTextAlfa(upgradeButton.TextList, true);
        }

        public void UpdateProgressBar(float progressValue = 0)
        {
            int value = Mathf.RoundToInt(progressValue * 100);
            string text = value > 0
                ? string.Concat(value, "%")
                : string.Empty;
            _gameUIView.ProgressBarView.ProgressSprite.fillAmount = progressValue;
            _gameUIView.ProgressBarView.ProgressValue.text = text;
        }

        public void ChangeGoldUi(int currentAmount)
        {
            _gameUIView.GoldTMP.text = currentAmount.ToString();
        }


        public async Task ChangeGemsUI(int amount)
        {
            //await Task.Delay(100);//gems fly animation
            _gameUIView.GemsTMP.text = amount.ToString();
        }

        public async Task ChangePowerUi(string amount)
        {
            // await Task.Delay(100);//power change animation
            _gameUIView.PowerTMP.text = amount;
        }

        public async Task ChangeDiceRollsUi(string amount)
        {
            // await Task.Delay(100);//dice change animation
            _gameUIView.DiceRollsTMP.text = amount;
        }

        public async Task ChangePowerUpgradeCostUi(string cost)
        {
            //await Task.Delay(100);//cost change animation
            _gameUIView.UpgradeCostTMP.text = cost;
        }

        public async Task ChangeMergeLevelButtonUi(string level)
        {
            // await Task.Delay(100);
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

            ActivateUiInteraction(_playerProfile.Stats.PowerUpgradeAvailable);
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

        private void SetRollButton(bool fightWin)
        {
            var rollButton = _gameUIView.RollButtonView;
            rollButton.ButtonText.text = fightWin
                ? Literal.ButtonText_Roll
                : Literal.ButtonText_Fight;

            rollButton.ButtonImage.sprite = fightWin
                ? rollButton.ImageSpriteDefault
                : rollButton.ImageSpriteFight;
        }

        private void DiceRoll()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.Selection);
            OnDiceRollClickEvent?.Invoke();
        }

        private void UpgradePower()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.Selection);
            OnUpgrdePowerClickEvent?.Invoke();
        }

        private void PlayMerge()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.Selection);
            OnPlayMergeButtonClicked?.Invoke();
        }

// Haptic types
//
// Selection : a light vibration on Android, and a light impact on iOS
// Success : a light then heavy vibration on Android, and a success impact on iOS
// Warning : a heavy then medium vibration on Android, and a warning impact on iOS
// Failure : a medium / heavy / heavy / light vibration pattern on Android, and a failure impact on iOS
// Light : a light impact on iOS and a short and light vibration on Android.
// Medium : a medium impact on iOS and a medium and regular vibration on Android
// Heavy : a heavy impact on iOS and a long and heavy vibration on Android
// Rigid : a short and hard impact
// Soft : a slightly longer and softer impact
    }
}
