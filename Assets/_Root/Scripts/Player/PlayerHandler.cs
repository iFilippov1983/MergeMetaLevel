using Data;
using Level;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Player
{
    internal class PlayerHandler
    {
        private const float AllowableValue = 0.1f;
        private PlayerProfile _playerProfile;
        private Vector3 _playerInitialPosition;
        private Vector3 _currentPosition;
        private Vector3 _targetPosition;

        private GameObject _playerPrefab;
        private PlayerView _playerView;

        private List<CellProperties> _currenRoute;

        public Action<int> OnPlayerCurrentCellIDChange;
        public Action<List<CellProperties>> OnPlayerCurrentRouteChange;
        public Func<int, Vector3> OnTargetPositionCall;

        public bool hasArrived = false;

        public PlayerHandler(GameData gameData, PlayerProfile playerProfile)
        {
            _playerPrefab = gameData.PlayerData.PlayerPrefab;
            _playerProfile = playerProfile;
            _playerInitialPosition = gameData.LevelData.CellsViews[_playerProfile.CurrentCellID.Value].transform.position;
            InitPlayer(_playerInitialPosition);//TODO: insert position from cash

            _playerProfile.CurrentCellID.SubscribeOnChange(OnSetCurrentCellID);
            //OnSetCurrentCellID(_playerProfile.CurrentCellID.Value);
            _playerProfile.CurrentRoute.SubscribeOnChange(OnSetRoute);
            //OnPlayerCurrentRouteChange(_playerProfile.CurrentRoute.Value);
        }

        public void InitPlayer(Vector3 playerInitPosition)
        {
            var playerObject = GameObject.Instantiate(_playerPrefab, playerInitPosition, Quaternion.identity);
            _playerView = playerObject.GetComponent<PlayerView>();
        }

        private async void OnSetRoute(List<CellProperties> cellViews)
        {
            hasArrived = false;
            foreach (var cell in cellViews)
            {
                if(OnTargetPositionCall != null)
                    _targetPosition = OnTargetPositionCall.Invoke(cell.ID);
                await SetDestinationAndMove(_targetPosition);
                _playerProfile.CurrentCellID.Value = cell.ID;
            }

            hasArrived = true;
        }

        private void OnSetCurrentCellID(int id)
        { 
        
        }

        private async Task SetDestinationAndMove(Vector3 position)
        {
            _playerView.NavMeshAgent.SetDestination(position);
            await Task.Delay(1000);
        }
    }
}
