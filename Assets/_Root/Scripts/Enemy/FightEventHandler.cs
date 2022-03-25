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
        private EnemyView _enemyView;
        private PlayerProfile _playerProfile;

        public Action<int> OnPlayerHitsEnemy;
        public Action<int> OnEnemyHitsPlayer;

        public FightEventHandler(EnemiesData enemiesData, AnimationHandler animationHandler, PlayerProfile playerProfile)
        {
            _animationHandler = animationHandler;
            _playerProfile = playerProfile;
        }

        public async Task ApplyFight
            (
            EnemyProperties enemyProperties,
            Action OnFightEvent
            //Action<EnemyProperties> OnFightEvent
            )
        {
            OnFightEvent?.Invoke();

            int playerHealth = _playerProfile.Stats.Health;
            int playerPower = _playerProfile.Stats.Power;
            int enemyHealth = enemyProperties.Stats.Health;
            int enemyPower = enemyProperties.Stats.Power;

            while (playerHealth > 0 && enemyHealth > 0)
            {
                enemyHealth -= playerPower;
                OnPlayerHitsEnemy?.Invoke(enemyHealth);
                await _animationHandler.PlayerHitsEnemyAnimation(enemyHealth <= 0);

                if (enemyHealth <= 0) break;

                playerHealth -= enemyPower;
                OnEnemyHitsPlayer?.Invoke(playerHealth);
                await _animationHandler.EnemyHitsPlayerAnimation(playerHealth <= 0);
            }

            bool result = playerHealth > 0;
            _playerProfile.Stats.LastFightWinner = result;
        }
    }
}
