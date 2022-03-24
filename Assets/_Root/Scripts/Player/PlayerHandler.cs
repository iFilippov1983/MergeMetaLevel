using Data;
using Game;
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
        private InfoHandler _infoHandler;
        private PlayerAnimationController _playerAnimController;
        private GameObject _infoPrefab;


        public PlayerView PlayerView => _playerView;
        public PlayerAnimationController PlayerAnimController => _playerAnimController;

        public PlayerHandler(GameData gameData, int initialCellId)
        {
            _playerPrefab = gameData.PlayerData.PlayerPrefab;
            _playerInitialPosition = gameData.LevelData.CellsViews[initialCellId].transform.position;
            InitPlayer(_playerInitialPosition);//TODO: insert position from cash
            _infoHandler = new InfoHandler(Camera.main);
            _infoPrefab = gameData.PlayerData.InfoPrefab;
        }

        public async Task SetDestinationAndMove(Vector3 position)
        {
            _playerView.NavMeshAgent.SetDestination(position);
            _playerView.Animator.SetBool(PlayerState.IsRunning, true);
            var transform = _playerView.transform;
            while(Vector3.SqrMagnitude(transform.position - position) > 0.2f * 0.2f)
                await Task.Yield();
        }

        public void InitHit(int enemyRemainingHealth)
        {
            _playerView.Animator.SetBool(PlayerState.IsAttacking, true);
        }

        public void InitGotHit(int playerRemainingHealth)
        {
            if (playerRemainingHealth <= 0)
                _playerView.Animator.SetBool(PlayerState.IsDefeated, true);
            else
                _playerView.Animator.SetBool(PlayerState.GotHit, true);

            //TODO: use remainingHealth to display on bar
        }

        public void InitDeath()
        { 
            
        }

        private void InitPlayer(Vector3 playerInitPosition)
        {
            var playerObject = GameObject.Instantiate(_playerPrefab, playerInitPosition, Quaternion.identity);
            _playerView = playerObject.GetComponent<PlayerView>();
            _playerAnimController = playerObject.GetComponent<PlayerAnimationController>();
        }

        internal void PrepareToFight(int power, int health)
        {
            _infoHandler.InitInformation
                (_infoPrefab, _playerView.transform.position, power, health);
        }

        internal void FinishFight()
        {
            _infoHandler.DestroyInformation();
        }
    }
}
