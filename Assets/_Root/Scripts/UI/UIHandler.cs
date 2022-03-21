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

        public Action OnDiceRollClickEvent;

        public UIHandler(UIData uiData, Transform uiContainer)
        {
            _uiContainer = uiContainer;
            _uiData = uiData;
        }
        public void InitializeUI(int coinsAmount, int gemsAmount, int diceRollsAmount, int power)
        {
            var uiObject = Object.Instantiate(_uiData.GameUIPrefab, _uiContainer);
            _gameUIView = uiObject.GetComponent<GameUIView>();
            _gameUIView.Init(DiceRoll);
            _gameUIView.CoinsText.text = coinsAmount.ToString();
            _gameUIView.GemsText.text = gemsAmount.ToString();
            _gameUIView.DiceRollsText.text = diceRollsAmount.ToString();
            _gameUIView.PowerText.text = power.ToString();
        }

        public async Task PlayDiceRollAnimation(int number) //animation ID
        {
            _gameUIView.NumberText.text = number.ToString();
            _gameUIView.NumberText.gameObject.SetActive(true);
            await Task.Delay(1000);
            _gameUIView.NumberText.text = string.Empty;
            _gameUIView.NumberText.gameObject.SetActive(false);
        }

        public async Task PlayDiceUseAnimation()
        {
            _gameUIView.NumberText.text = UiString.NextAttempt;
            _gameUIView.NumberText.gameObject.SetActive(true);
            await Task.Delay(1000);
            _gameUIView.NumberText.text = string.Empty;
            _gameUIView.NumberText.gameObject.SetActive(false);
        }

        public async Task DisplayText(string text)
        {
            _gameUIView.NumberText.text = text;
            _gameUIView.NumberText.gameObject.SetActive(true);
            await Task.Delay(1000);
            _gameUIView.NumberText.text = string.Empty;
            _gameUIView.NumberText.gameObject.SetActive(false);
        }

        public void ActivateUiInteraction()
        {
            _gameUIView.RollButton.interactable = true;
        }

        public void DesactivateUiInteraction()
        {
            _gameUIView.RollButton.interactable = false;
        }

        public async Task ChangeCoinsUI(int amount)
        { 
            await Task.Delay(100);//coins fly animation
            _gameUIView.CoinsText.text = amount.ToString();
        }

        public async Task ChangeGemsUI(int amount)
        {
            await Task.Delay(100);//gems fly animation
            _gameUIView.GemsText.text = amount.ToString();
        }

        internal async Task ChangeRollsUI(int amount)
        {
            await Task.Delay(100);//dice change animation
            _gameUIView.DiceRollsText.text = amount.ToString();
        }

        private void DiceRoll()
        { 
            OnDiceRollClickEvent?.Invoke();
        }
    }
}
