using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Enemy
{
    internal class FightEventHandler
    {
        private EnemyView _enemyView;
        private EnemyAnimationControler _animationController;
        private EnemiesData _enemyData;
        private PlayerProfile _playerProfile;
        private Dictionary<EnemyType, GameObject> _enemyPrefabs;

        public FightEventHandler(EnemiesData enemiesData, PlayerProfile playerProfile)
        {
            _enemyData = enemiesData;
            _playerProfile = playerProfile;
            _enemyPrefabs = MakeEnemyPrefabsDictionary(_enemyData.EnemiesPrefabs);
        }

        public async Task ApplyFight
            (
            EnemyProperties enemyProperties,
            Action<EnemyProperties> OnFightEvent,
            Transform placeToSpawn = null, 
            Transform placeToFight = null, 
            bool initEnemy = false
            )
        {
            if (initEnemy)
                await InitializeEnemy(enemyProperties, placeToSpawn, placeToFight);

            OnFightEvent?.Invoke(enemyProperties);
            bool result = await Fight(_playerProfile.Stats, enemyProperties.EnemyStats);

            _enemyView.Animator.SetBool(EnemyState.IsKilled, result);
            _playerProfile.Stats.LastFightWinner = result;
        }

        public async Task DestroyEnemy()
        {
            bool animationCompleted = _animationController.deathAnimationFinished;
            while (!animationCompleted)
            {
                await Task.Yield();
                animationCompleted = _animationController.deathAnimationFinished;
            }
            Object.Destroy(_enemyView.gameObject);
        }

        private async Task<bool> Fight(PlayerStats player, EnemyStats enemy)
        {
            await Task.Delay(2000);//fight animation
            return player.Power > enemy.Power 
                ? true 
                : false;
        }

        private async Task InitializeEnemy(EnemyProperties enemyProperties, Transform placeToSpawn, Transform placeToFight)
        {
            GameObject enemy = _enemyPrefabs[enemyProperties.EnemyType];
            var enemyObject = Object.Instantiate(enemy, placeToSpawn.position, placeToSpawn.rotation);

            _enemyView = enemyObject.GetComponent<EnemyView>();
            _enemyView.NavMeshAgent.SetDestination(placeToFight.position);

            _animationController = enemyObject.GetComponent<EnemyAnimationControler>();
            bool animationCompleted = _animationController.appearAnimationFinished;
            while (!animationCompleted)
            {
                await Task.Yield();
                animationCompleted = _animationController.appearAnimationFinished;
            }
        }

        private Dictionary<EnemyType, GameObject> MakeEnemyPrefabsDictionary(List<GameObject> enemyiesPrefabs)
        {
            var dictionary = new Dictionary<EnemyType, GameObject>();
            foreach (var enemy in enemyiesPrefabs)
            {
                if (enemy != null)
                {
                    var view = enemy.GetComponent<EnemyView>();
                    var type = view.EnemyType;
                    dictionary.Add(type, enemy);
                }
            }
            return dictionary;
        }
    }
}
