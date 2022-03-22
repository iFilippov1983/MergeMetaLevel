using Data;
using GameUI;
using Level;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Player
{
    internal class PlayerHandler
    {
        private Vector3 _playerInitialPosition;
        private GameObject _playerPrefab;
        private PlayerView _playerView;

        public PlayerView PlayerView => _playerView;

        public PlayerHandler(GameData gameData, int initialCellId)
        {
            _playerPrefab = gameData.PlayerData.PlayerPrefab;
            _playerInitialPosition = gameData.LevelData.CellsViews[initialCellId].transform.position;
            InitPlayer(_playerInitialPosition);//TODO: insert position from cash
        }

        public async Task SetDestinationAndMove(Vector3 position)
        {
            _playerView.NavMeshAgent.SetDestination(position);
            var transform = _playerView.transform;
            while(Vector3.SqrMagnitude(transform.position - position) > 0.2f * 0.2f)
                await Task.Yield();
        }

        public async Task RespawnPlayer(Vector3 position)
        {
            await Task.Delay(100); //respawn animation
            _playerView.transform.position = position;
        }

        private void InitPlayer(Vector3 playerInitPosition)
        {
            var playerObject = GameObject.Instantiate(_playerPrefab, playerInitPosition, Quaternion.identity);
            _playerView = playerObject.GetComponent<PlayerView>();
        }
    }
}
