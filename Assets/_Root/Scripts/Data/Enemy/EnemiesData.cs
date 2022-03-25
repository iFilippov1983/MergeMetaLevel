using UnityEditor;
using UnityEngine;
using Tool;
using System.Collections.Generic;
using System;

namespace Data
{
    [CreateAssetMenu(menuName = "GameData/EnemiesData", fileName = "EnemiesData")]
    public class EnemiesData : ScriptableObject
    {
        [SerializeField] private string _enemyOnePrefabPath;
        [SerializeField] private string _enemyTwoPrefabPath;

        private List<GameObject> _enemiesPrefabs;

        public List<GameObject> EnemiesPrefabs => GetEnemiesPrefabs();

        private List<GameObject> GetEnemiesPrefabs()
        {
            if (_enemiesPrefabs == null || _enemiesPrefabs.Count == 0)
            {
                _enemiesPrefabs = new List<GameObject>();
                _enemiesPrefabs.Add(EnemyOnePrefab);
                _enemiesPrefabs.Add(EnemyTwoPrefab);
            }
            return _enemiesPrefabs;
        }

        private GameObject EnemyOnePrefab =>
            Resources.Load<GameObject>(string.Concat(ResourcePath.EnemyPrefabsFolder, _enemyOnePrefabPath));
        //{
        //    get
        //    {
        //        if (_enemyBarbarianPrefab == null) _enemyBarbarianPrefab =
        //                     Resources.Load<GameObject>(string.Concat(ResourcePath.PrefabsFolder, _enemyBarbarianPrefabPath));
        //        return _enemyBarbarianPrefab;
        //    }
        //}

        private GameObject EnemyTwoPrefab =>
            Resources.Load<GameObject>(string.Concat(ResourcePath.EnemyPrefabsFolder, _enemyTwoPrefabPath));
        //{
        //    get
        //    {
        //        if (_enemyRoguePrefab == null) _enemyRoguePrefab =
        //                    Resources.Load<GameObject>(string.Concat(ResourcePath.PrefabsFolder, _enemyRoguePrefabPath));
        //        return _enemyRoguePrefab;
        //    }
        //}


        private void OnDisable()
        {
            if(_enemiesPrefabs != null) _enemiesPrefabs.Clear();
        }
    }
}