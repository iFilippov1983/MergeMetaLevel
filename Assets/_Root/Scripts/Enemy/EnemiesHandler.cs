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
    internal class EnemiesHandler
    {
        private EnemyView _enemy;
        private EnemiesData _enemyData;
        private PlayerProfile _playerProfile;
        private Dictionary<EnemyType, GameObject> _enemyPrefabs;

        public EnemiesHandler(EnemiesData enemiesData, PlayerProfile playerProfile)
        {
            _enemyData = enemiesData;
            _playerProfile = playerProfile;
            _enemyPrefabs = MakeEnemyPrefabsDictionary(_enemyData.EnemiesPrefabs);
        }

        public async Task FightEvent(EnemyProperties enemyProperties, Transform placeToSpawn, Transform placeToFight, Action<EnemyProperties> OnFightEvent)
        {
            GameObject enemy = _enemyPrefabs[enemyProperties.EnemyType];
            await InitializeEnemy(enemy, placeToSpawn, placeToFight);
            OnFightEvent?.Invoke(enemyProperties);
            bool result = await Fight(_playerProfile.Stats, enemyProperties.EnemyStats);
            _playerProfile.Stats.LastFightWinner = result;
        }

        public async Task<bool> Fight(PlayerStats player, EnemyStats enemy)
        {
            await Task.Delay(2000);//fight animation
            if (player.Power > enemy.Power) return true;
            else return false;
        }

        private async Task InitializeEnemy(GameObject prefab, Transform placeToSpawn, Transform placeToFight)
        { 
            var enemyObject = Object.Instantiate(prefab, placeToSpawn.position, placeToSpawn.rotation);
            var view = enemyObject.GetComponent<EnemyView>();
            view.NavMeshAgent.SetDestination(placeToFight.position);

            while (Vector3.SqrMagnitude(enemyObject.transform.position - placeToFight.position) > 0.1f * 0.1f)
                await Task.Yield();
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
