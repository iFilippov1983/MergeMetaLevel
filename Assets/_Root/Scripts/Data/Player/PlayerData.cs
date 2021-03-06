using UnityEngine;
using Tool;
using System;

namespace Data
{
    [CreateAssetMenu(menuName = "GameData/PlayerData", fileName = "PlayerData")]
    public class PlayerData : ScriptableObject
    {
        [SerializeField] private string _playerPrefabName;
        [SerializeField] private string _infoPrefabName;

        private GameObject _playerPrefab;
        private GameObject _infoPrefab;

        public GameObject PlayerPrefab => 
            Resources.Load<GameObject>(string.Concat(ResourcePath.PrefabsFolder, _playerPrefabName));
        //{
        //    get
        //    {
        //        if (_playerPrefab == null) _playerPrefab =
        //                 Resources.Load<GameObject>(string.Concat(ResourcePath.PrefabsFolder, _playerPrefabName));
        //        return _playerPrefab;
        //    }
        //}

        public GameObject InfoPrefab
        {
            get
            {
                if (_infoPrefab == null) _infoPrefab =
                             Resources.Load<GameObject>(string.Concat(ResourcePath.PrefabsFolder, _infoPrefabName));
                return _infoPrefab;
            }
        }
    }
}