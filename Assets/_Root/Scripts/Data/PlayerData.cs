using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "GameData/PlayerData", fileName = "PlayerData")]
    public class PlayerData : ScriptableObject
    {
        private const string PrefabsFolderPath = "Prefabs/";

        [SerializeField] private string _playerPrefabPath;

        private GameObject _playerPrefab;

        public GameObject PlayerPrefab
        {
            get
            {
                if (_playerPrefab == null) _playerPrefab =
                         Resources.Load<GameObject>(string.Concat(PrefabsFolderPath, _playerPrefabPath));
                return _playerPrefab;
            }
        }
    }
}