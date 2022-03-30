using Data;
using Enemy;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game
{
    internal class FightEventHandler
    {
        private AnimationHandler _animationHandler;
        private PlayerProfile _playerProfile;

        private int _playerHealth;
        private int _enemyHealth;

        private Action<int> _playerGetHitEvent;
        private Action<int> _enemyGetHitEvent;

        public FightEventHandler(EnemiesData enemiesData, AnimationHandler animationHandler, PlayerProfile playerProfile)
        {
            _animationHandler = animationHandler;
            _playerProfile = playerProfile;
        }

        public async Task ApplyFight
            (
            Action<int> PlayerGetHit, 
            Action<int> EnemyGetHit, 
            EnemyProperties enemyProperties
            )
        {
            _playerHealth = _playerProfile.Stats.Health;
            int playerPower = _playerProfile.Stats.Power;
            _enemyHealth = enemyProperties.Stats.Health;
            int enemyPower = enemyProperties.Stats.Power;
            _playerGetHitEvent = PlayerGetHit;
            _enemyGetHitEvent = EnemyGetHit;

            while (_playerHealth > 0 && _enemyHealth > 0)
            {
                _enemyHealth -= playerPower;
                await _animationHandler.AnimateHit(true, _enemyHealth <= 0, GotHitEvent);
                
                if (_enemyHealth <= 0) break;

                _playerHealth -= enemyPower;
                await _animationHandler.AnimateHit(false, _playerHealth <= 0, GotHitEvent);
                PlayerGetHit?.Invoke(_playerHealth);
            }

            bool result = _playerHealth > 0;
            _playerProfile.Stats.LastFightWinner = result;
        }

        private void GotHitEvent(bool playerAttacking)
        { 
            if(playerAttacking)
                _enemyGetHitEvent?.Invoke(_enemyHealth);
            else
                _playerGetHitEvent?.Invoke(_playerHealth);

        }
    }
}
