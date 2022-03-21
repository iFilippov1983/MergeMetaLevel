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
        [SerializeField] private string _enemyBarbarianPrefabPath;
        [SerializeField] private string _enemyRoguePrefabPath;
        [SerializeField] private string _enemyWitchPrefabPath;

        private GameObject _enemyBarbarianPrefab;
        private GameObject _enemyRoguePrefab;
        private GameObject _enemyWitchPrefab;
        private List<GameObject> _enemiesPrefabs;

        public List<GameObject> EnemiesPrefabs => GetEnemiesPrefabs();

        private List<GameObject> GetEnemiesPrefabs()
        {
            if (_enemiesPrefabs.Count == 0)
            {
                _enemiesPrefabs = new List<GameObject>();
                _enemiesPrefabs.Add(EnemyBarbarianPrefab);
                _enemiesPrefabs.Add(EnemyRoguePrefab);
                _enemiesPrefabs.Add(EnemyWitchPrefab);
            }
            return _enemiesPrefabs;
        }

        private GameObject EnemyBarbarianPrefab
        {
            get
            {
                if (_enemyBarbarianPrefab == null) _enemyBarbarianPrefab =
                             Resources.Load<GameObject>(string.Concat(ResourcePath.PrefabsFolder, _enemyBarbarianPrefabPath));
                return _enemyBarbarianPrefab;
            }
        }

        private GameObject EnemyRoguePrefab
        {
            get
            {
                if (_enemyRoguePrefab == null) _enemyRoguePrefab =
                            Resources.Load<GameObject>(string.Concat(ResourcePath.PrefabsFolder, _enemyRoguePrefabPath));
                return _enemyRoguePrefab;
            }
        }

        private GameObject EnemyWitchPrefab
        {
            get
            {
                if (_enemyWitchPrefab == null) _enemyWitchPrefab =
                             Resources.Load<GameObject>(string.Concat(ResourcePath.PrefabsFolder, _enemyWitchPrefabPath));
                return _enemyWitchPrefab;
            }
        }

        private void OnDisable()
        {
            _enemiesPrefabs.Clear();
        }
    }
}