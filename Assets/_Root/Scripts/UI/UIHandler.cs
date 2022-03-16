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

        public Action OnDiceRollClick;

        public UIHandler(UIData uiData, Transform uiContainer)
        {
            _uiContainer = uiContainer;
            _uiData = uiData;
            InitializeUI();
        }

        public async Task PlayDiceRollAnimation(int number) //animation ID
        {
            _gameUIView.NumberText.text = number.ToString();
            _gameUIView.NumberText.gameObject.SetActive(true);
            await Task.Delay(1000);
            _gameUIView.NumberText.text = string.Empty;
            _gameUIView.NumberText.gameObject.SetActive(false);
        }

        public void ActivateUI()
        {
            _gameUIView.RollButton.interactable = true;
        }

        public void DesactivateUI()
        {
            _gameUIView.RollButton.interactable = false;
        }

        private void DiceRoll()
        { 
            OnDiceRollClick?.Invoke();
        }
        
        private void InitializeUI()
        {
            var uiObject = Object.Instantiate(_uiData.GameUIPrefab, _uiContainer);
            _gameUIView = uiObject.GetComponent<GameUIView>();
            _gameUIView.Init(DiceRoll);
        }
    }
}
