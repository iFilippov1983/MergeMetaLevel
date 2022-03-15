using Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Player
{
    internal class PlayerAPI
    {
        private PlayerHandler _playerHandler;
        private PlayerProfile _playerProfile;
        private Vector3 _playerInitialPosition;

        public PlayerAPI(GameData gameData, PlayerProfile playerProfile)
        {
            _playerHandler = new PlayerHandler(gameData.PlayerData);
            _playerProfile = playerProfile;
            _playerInitialPosition = gameData.LevelData.CellsViews[_playerProfile.CurrentCellID.Value].transform.position;
            _playerHandler.InitPlayer(_playerInitialPosition);//TODO: insert position from cash
        }

        public void MoveTo(Vector3 position)
        {
            _playerHandler.SetDestination(position);
        }
    }
}
