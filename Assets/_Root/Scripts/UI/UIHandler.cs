using System;
using System.Threading.Tasks;
using UnityEngine;
using Data;
using Object = UnityEngine.Object;

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

            await ChangeDiceRollsUi(--_playerProfile.Stats.DiceRolls);
        }

        public async Task PlayDiceUseAnimation() //when next fight attempt
        {
            _gameUIView.MainText.text = UiString.NextAttempt;
            _gameUIView.MainText.gameObject.SetActive(true);
            await Task.Delay(1000);
            _gameUIView.MainText.text = string.Empty;
            _gameUIView.MainText.gameObject.SetActive(false);

            await ChangeDiceRollsUi(--_playerProfile.Stats.DiceRolls);
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
            _gameUIView.RollButton.interactable = true;
            _gameUIView.UpgradePowerButton.gameObject.SetActive(_playerProfile.Stats.PowerUpgradeAvailable);
            _gameUIView.UpgradePowerButton.interactable = true;
            _gameUIView.PlayMergeButton.interactable = true;
        }

        public void DesactivateUiInteraction()
        {
            _gameUIView.RollButton.interactable = false;
            _gameUIView.UpgradePowerButton.interactable = false;
            _gameUIView.PlayMergeButton.interactable= false;
        }

        public async Task ChangeCoinsUi(int amount)
        { 
            await Task.Delay(100);//coins fly animation
            _gameUIView.CoinsText.text = amount.ToString();
        }

        public async Task ChangeGemsUI(int amount)
        {
            await Task.Delay(100);//gems fly animation
            _gameUIView.GemsText.text = amount.ToString();
        }

        internal async Task ChangePowerUi(int amount)
        {
            await Task.Delay(100);//power change animation
            _gameUIView.PowerText.text = amount.ToString();
        }

        internal async Task ChangeDiceRollsUi(int amount)
        {
            await Task.Delay(100);//dice change animation
            _gameUIView.DiceRollsText.text = amount.ToString();
        }

        private void InitializeUI()
        {
            PlayerStats stats = _playerProfile.Stats;
            var uiObject = Object.Instantiate(_uiData.GameUIPrefab, _uiContainer);
            _gameUIView = uiObject.GetComponent<GameUIView>();
            _gameUIView.Init(DiceRoll, UpgradePower, PlayMerge);
            _gameUIView.CoinsText.text = stats.Gold.ToString();
            _gameUIView.GemsText.text = stats.Gems.ToString();
            _gameUIView.DiceRollsText.text = stats.DiceRolls.ToString();
            _gameUIView.PowerText.text = stats.Power.ToString();
            _gameUIView.UpgradePowerButton.gameObject.SetActive(stats.PowerUpgradeAvailable);
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
