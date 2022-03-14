using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Player
{
    internal class PlayerAPI
    {
        private PlayerHandler _playerHandler;
        private Vector3 _playerInitialPosition;

        public PlayerAPI(GameData gameData)
        {
            _playerHandler = new PlayerHandler(gameData.PlayerData);
            _playerInitialPosition = gameData.LevelData.CellsViews[0].transform.position;
        }

        public void InitializePlayer()
        {
            _playerHandler.InitPlayer(_playerInitialPosition);//TODO: insert position from cash
            //+ other initializations
        }

        public async Task MoveTo(Vector3 position)
        {
            _playerHandler.SetDestination(position);
        }
    }
}
