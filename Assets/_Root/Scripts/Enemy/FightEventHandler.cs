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
        private EnemiesData _enemyData;
        private PlayerProfile _playerProfile;
        private Dictionary<EnemyType, GameObject> _enemyPrefabs;

        public FightEventHandler(EnemiesData enemiesData, PlayerProfile playerProfile)
        {
            _enemyData = enemiesData;
            _playerProfile = playerProfile;
            _enemyPrefabs = MakeEnemyPrefabsDictionary(_enemyData.EnemiesPrefabs);
        }

        public async Task ApplyFight(EnemyProperties enemyProperties, Transform placeToSpawn, Transform placeToFight, Action<EnemyProperties> OnFightEvent)
        {
            await InitializeEnemy(enemyProperties, placeToSpawn, placeToFight);

            OnFightEvent?.Invoke(enemyProperties);
            bool result = await Fight(_playerProfile.Stats, enemyProperties.EnemyStats);

            _enemyView.Animator.SetBool(EnemyState.IsKilled, result);
            _playerProfile.Stats.LastFightWinner = result;
        }

        public async Task ApplyNextAttemptFight(EnemyProperties enemyProperties, Action<EnemyProperties> OnFightEvent)
        {
            OnFightEvent.Invoke(enemyProperties);
            bool result = await Fight(_playerProfile.Stats, enemyProperties.EnemyStats);
            _enemyView.Animator.SetBool(EnemyState.IsKilled, result);
            _playerProfile.Stats.LastFightWinner = result;
        }

        public async Task DisposeEnemy()
        {
            await Task.Delay(2000); //enemy death animation
            Object.Destroy(_enemyView.gameObject);
        }

        private async Task<bool> Fight(PlayerStats player, EnemyStats enemy)
        {
            await Task.Delay(2000);//fight animation
            if (player.Power > enemy.Power) return true;
            else return false;
        }

        private async Task InitializeEnemy(EnemyProperties enemyProperties, Transform placeToSpawn, Transform placeToFight)
        {
            GameObject enemy = _enemyPrefabs[enemyProperties.EnemyType];
            var enemyObject = Object.Instantiate(enemy, placeToSpawn.position, placeToSpawn.rotation);
            _enemyView = enemyObject.GetComponent<EnemyView>();
            _enemyView.PowerText.text = enemyProperties.EnemyStats.Power.ToString();
            _enemyView.NavMeshAgent.SetDestination(placeToFight.position);

            //bool isOnPlace = Vector3.SqrMagnitude(enemyObject.transform.position - placeToFight.position) > 0.1f * 0.1f;
            //bool isOnTheWay = view.Animator.GetCurrentAnimatorStateInfo(0).loop;
            //while (isOnTheWay)
            //    await Task.Yield();

            await Task.Delay(4500);
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
