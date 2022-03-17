using UnityEngine;
using Tool;

namespace Data
{
    [CreateAssetMenu(menuName = "GameData/PlayerData", fileName = "PlayerData")]
    public class PlayerData : ScriptableObject
    {
        [SerializeField] private string _playerPrefabName;

        private GameObject _playerPrefab;

        public GameObject PlayerPrefab
        {
            get
            {
                if (_playerPrefab == null) _playerPrefab =
                         Resources.Load<GameObject>(string.Concat(ResourcePath.PrefabsFolder, _playerPrefabName));
                return _playerPrefab;
            }
        }
    }
}