﻿using Data;
using Game;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Enemy
{
    internal class EnemyHandler
    {
        private EnemyView _enemyView;
        private AnimationHandler _animationHandler;
        private UiInfoHandler _infoHandler;
        private Dictionary<EnemyType, GameObject> _enemyPrefabs;
        private int _enemyFullHealthAmount;

        public EnemyHandler(EnemiesData enemiesData, AnimationHandler animationHandler)
        {
            _animationHandler = animationHandler;
            _infoHandler = new UiInfoHandler(Camera.main);
            _enemyPrefabs = MakeEnemyPrefabsDictionary(enemiesData.EnemiesPrefabs);
        }

        public async Task InitializeEnemy(EnemyProperties enemyProperties, Transform placeToSpawn)
        {
            GameObject enemy = _enemyPrefabs[enemyProperties.EnemyType];
            var enemyObject = Object.Instantiate(enemy, placeToSpawn.position, placeToSpawn.rotation);

            _animationHandler.SetEnemyToControl(enemyObject);
            _enemyView = enemyObject.GetComponent<EnemyView>();

            _enemyFullHealthAmount = enemyProperties.Stats.Health;

            await _animationHandler.HandleEnemyAppearAnimation();
            _infoHandler.SetInformation
                (enemyProperties.InfoPrefab, enemyObject.transform.position, enemyProperties.Stats.Power, enemyProperties.Stats.Health);
        }

        public void InitHealthBar()
        {
            _infoHandler.InitInformation();  
        }

        public void OnFightFinishEvent(bool playerWins)
        {
            if(playerWins)
                Object.Destroy(_enemyView.gameObject);
            _infoHandler.DestroyInformation();
        }

        public void OnGetHitEvent(int enemyRemainingHealth)
        {
            if (enemyRemainingHealth <= 0)
                _infoHandler.SetHealth(0, 0f);
            else
            {
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
