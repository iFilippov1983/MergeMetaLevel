using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Tool;
using Data;
using Object = UnityEngine.Object;

namespace GameUI
{
    internal class UIHandler
    {
        private Transform _uiContainer;
        private UIData _uiData;
        private GameUIView _gameUIView;
        private int _numberForDice;

        private Action OnDiceRollFinshed;
        public Func<int> NumberForDiceCall;

        public UIHandler(UIData uiData, Action OnDiceRollAction, Transform uiContainer)
        {
            _uiContainer = uiContainer;
            _uiData = uiData;
            OnDiceRollFinshed += OnDiceRollAction;
            InitializeUI();
        }

        private void InitializeUI()
        {
            var uiObject = Object.Instantiate(_uiData.GameUIPrefab, _uiContainer);
            _gameUIView = uiObject.GetComponent<GameUIView>();
            _gameUIView.Init(DiceRoll);
        }

        private async void DiceRoll()
        {
            if (NumberForDiceCall != null)
                _numberForDice = NumberForDiceCall.Invoke();
            await PlayDiceRollAnimation(_numberForDice);
            OnDiceRollFinshed?.Invoke();
        }


        private async Task PlayDiceRollAnimation(int number) //animation ID
        {
            _gameUIView.NumberText.text = number.ToString();
            _gameUIView.NumberText.gameObject.SetActive(true);
            await Task.Delay(1000);
            _gameUIView.NumberText.text = string.Empty;
            _gameUIView.NumberText.gameObject.SetActive(false);
        }
    }
}
