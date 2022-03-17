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

        public Action OnDiceRollClick;

        public UIHandler(UIData uiData, Transform uiContainer, PlayerProfile playerProfile)
        {
            _uiContainer = uiContainer;
            _uiData = uiData;
            _playerProfile = playerProfile;
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

        public async Task DefineResourceAndChangeUI(ResouceType resouceType, int amount)
        {
            switch (resouceType)
            {
                case ResouceType.Gold:
                    _playerProfile.CoinsAmount += amount;
                    _gameUIView.CoinsText.text = _playerProfile.CoinsAmount.ToString();
                    await Task.Delay(100);//temp
                    break;
                case ResouceType.Gem:
                    _playerProfile.GemsAmount += amount;
                    _gameUIView.GemsText.text = _playerProfile.GemsAmount.ToString();
                    await Task.Delay(100);//temp
                    break;
                default:
                    break;
            }
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
