using UnityEngine;
using Tool;
using System;
using Sirenix.OdinInspector;

namespace Data
{
    [CreateAssetMenu(menuName = "GameData/PlayerData", fileName = "PlayerData")]
    public class PlayerData : ScriptableObject
    {
        [SerializeField] 
        [InlineButton("SwitchCharacter", "Switch char")]
        private string _playerPrefabName;
        [SerializeField] private string _infoPrefabName;

        //private GameObject _playerPrefab;
        //private GameObject _infoPrefab;

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

        public GameObject InfoPrefab =>
            Resources.Load<GameObject>(string.Concat(ResourcePath.PrefabsFolder, _infoPrefabName));
        //{
        //    get
        //    {
        //        if (_infoPrefab == null) _infoPrefab =
        //                     Resources.Load<GameObject>(string.Concat(ResourcePath.PrefabsFolder, _infoPrefabName));
        //        return _infoPrefab;
        //    }
        //}

        private void SwitchCharacter()
        {
            _playerPrefabName = _playerPrefabName.Equals(ResourcePath.PlayerPrefabName)
                ? ResourcePath.PlayerElfPrefabName
                : ResourcePath.PlayerPrefabName;
        }
    }
}