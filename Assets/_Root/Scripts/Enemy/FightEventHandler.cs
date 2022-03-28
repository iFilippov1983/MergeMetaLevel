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
            int playerHealth = _playerProfile.Stats.Health;
            int playerPower = _playerProfile.Stats.Power;
            int enemyHealth = enemyProperties.Stats.Health;
            int enemyPower = enemyProperties.Stats.Power;

            while (playerHealth > 0 && enemyHealth > 0)
            {
                enemyHealth -= playerPower;
                await _animationHandler.AnimateHit(true, enemyHealth <= 0);
                EnemyGetHit?.Invoke(enemyHealth);

                if (enemyHealth <= 0) break;
                await Task.Delay(500);//timing animation delay

                playerHealth -= enemyPower;
                await _animationHandler.AnimateHit(false, playerHealth <= 0);
                PlayerGetHit?.Invoke(playerHealth);

                await Task.Delay(500);//timing animation delay
            }

            bool result = playerHealth > 0;
            _playerProfile.Stats.LastFightWinner = result;
        }
    }
}
