using Data;
using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Enemy
{
    internal class EnemyHandler
    {
        private EnemyView _enemyView;
        private AnimationHandler _animationHandler;
        private InfoHandler _infoHandler;
        private Dictionary<EnemyType, GameObject> _enemyPrefabs;
        private int _enemyFullHealthAmount;

        public EnemyHandler(EnemiesData enemiesData, AnimationHandler animationHandler)
        {
            _animationHandler = animationHandler;
            _infoHandler = new InfoHandler(Camera.main);
            _enemyPrefabs = MakeEnemyPrefabsDictionary(enemiesData.EnemiesPrefabs);
        }

        public async Task InitializeEnemy(EnemyProperties enemyProperties, Transform placeToSpawn, Transform placeToFight)
        {
            GameObject enemy = _enemyPrefabs[enemyProperties.EnemyType];
            var enemyObject = Object.Instantiate(enemy, placeToSpawn.position, placeToSpawn.rotation);

            _animationHandler.SetEnemyToControl(enemyObject);
            _enemyView = enemyObject.GetComponent<EnemyView>();
            _enemyView.NavMeshAgent.SetDestination(placeToFight.position);

            _enemyFullHealthAmount = enemyProperties.Stats.Health;

            await _animationHandler.EnemyAppearAnimation();
            _enemyView.NavMeshAgent.enabled = false;
            _infoHandler.InitInformation
                (enemyProperties.InfoPrefab, enemyObject.transform.position, enemyProperties.Stats.Power, enemyProperties.Stats.Health);
        }

        public void DoHit(int playerRemainingHealth)
        {
            _enemyView.Animator.SetBool(EnemyState.IsAttacking, true);
        }

        public async void GetHit(int enemyRemainingHealth)
        {
            if (enemyRemainingHealth <= 0)
            {
                _enemyView.Animator.SetBool(EnemyState.IsKilled, true);
                _infoHandler.SetHealth(0, 0f);

                await _animationHandler.EnemyDeathAnimation();

                Object.Destroy(_enemyView.gameObject);
                _infoHandler.DestroyInformation();
            }
            else
            {
                _enemyView.Animator.SetBool(EnemyState.GotHit, true);
                float fillAmount = (float)enemyRemainingHealth / (float)_enemyFullHealthAmount;
                _infoHandler.SetHealth(enemyRemainingHealth, fillAmount);
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
