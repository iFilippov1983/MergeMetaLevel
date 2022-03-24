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

            await _animationHandler.EnemyAppearAnimation();
            _enemyView.NavMeshAgent.enabled = false;
            _infoHandler.InitInformation
                (enemyProperties.InfoPrefab, enemyObject.transform.position, enemyProperties.Stats.Power, enemyProperties.Stats.Health);
        }

        public void InitHit(int playerRemainingHealth)
        {
            _enemyView.Animator.SetBool(EnemyState.IsAttacking, true);
        }

        public async void InitGotHit(int enemyRemainingHealth)
        {
            if (enemyRemainingHealth <= 0)
            {
                await Task.Delay(600);//temp
                _enemyView.Animator.SetBool(EnemyState.IsKilled, true);
                await _animationHandler.EnemyDeathAnimation();
                Object.Destroy(_enemyView.gameObject);
                _infoHandler.DestroyInformation();
            }
                
            //else
            //    _enemyView.Animator.SetBool(EnemyState.GotHit, true);
            //TODO: use remainingHealth to display on bar
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
