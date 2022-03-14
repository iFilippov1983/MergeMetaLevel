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
        private Vector3 _playerInitPosition;
        private PlayerView _playerView;

        public PlayerHandler(PlayerData playerData)
        {
            _playerData = playerData;
            _playerPrefab = _playerData.PlayerPrefab;
        }

        public void InitPlayer(Vector3 playerInitPosition)
        {
            var plObject = GameObject.Instantiate(_playerPrefab, playerInitPosition, Quaternion.identity);
            _playerView = plObject.GetComponent<PlayerView>();
        }

        public async Task SetDestination(Vector3 position)
        {
            _playerView.NavMeshAgent.SetDestination(position);
            //
        }

    }
}
