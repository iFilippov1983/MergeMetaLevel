using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Player
{
    internal class PlayerHandler
    {
        private PlayerData _playerData;
        private GameObject _playerPrefab;
        private PlayerView _playerView;

        public PlayerHandler(PlayerData playerData)
        {
            _playerData = playerData;
            _playerPrefab = _playerData.PlayerPrefab;
        }

        public void InitPlayer(Vector3 playerInitPosition)
        {
            var playerObject = GameObject.Instantiate(_playerPrefab, playerInitPosition, Quaternion.identity);
            _playerView = playerObject.GetComponent<PlayerView>();
        }

        public void SetDestination(Vector3 position)
        {
            _playerView.NavMeshAgent.SetDestination(position);
        }
    }
}
