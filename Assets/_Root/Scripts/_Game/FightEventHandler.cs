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

        private int _playerPower;
        private int _playerHealth;
        private int _enemyPower;
        private int _enemyHealth;

        private Action<int, int> _playerGetHitEvent;
        private Action<int, int> _enemyGetHitEvent;
        private Action _camShakeEvent;

        public FightEventHandler(AnimationHandler animationHandler, PlayerProfile playerProfile)
        {
            _animationHandler = animationHandler;
            _playerProfile = playerProfile;
        }

        public async Task ApplyFight
            (
            Action<int, int> PlayerGetHit, 
            Action<int, int> EnemyGetHit,
            Action ShakeCamera,
            EnemyProperties enemyProperties
            )
        {
            _playerHealth = _playerProfile.Stats.Health;
            _playerPower = _playerProfile.Stats.Power;
            _enemyHealth = enemyProperties.Stats.Health;
            _enemyPower = enemyProperties.Stats.Power;
            _playerGetHitEvent = PlayerGetHit;
            _enemyGetHitEvent = EnemyGetHit;
            _camShakeEvent = ShakeCamera;

            while (_playerHealth > 0 && _enemyHealth > 0)
            {
                _enemyHealth -= _playerPower;
                await _animationHandler.AnimateHit(true, _enemyHealth <= 0, GotHitEvent);
                
                if (_enemyHealth <= 0) break;

                _playerHealth -= _enemyPower;
                await _animationHandler.AnimateHit(false, _playerHealth <= 0, GotHitEvent);

            }

            bool result = _playerHealth > 0;
            _playerProfile.Stats.LastFightWinner = result;
        }

        private void GotHitEvent(bool playerAttacking)
        {
            _camShakeEvent?.Invoke();

            if(playerAttacking)
                _enemyGetHitEvent?.Invoke(_playerPower, _enemyHealth);
            else
                _playerGetHitEvent?.Invoke(_enemyPower, _playerHealth);

        }
    }
}
