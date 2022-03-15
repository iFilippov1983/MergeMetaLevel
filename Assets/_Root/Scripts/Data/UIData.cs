using UnityEngine;
using Tool;

namespace Data
{
    [CreateAssetMenu(menuName = "GameData/UIData", fileName = "UIData")]
    public class UIData : ScriptableObject
    {
        [SerializeField] private string _gameUIPrefabName;
        
        private GameObject _gameUI;

        public GameObject GameUIPrefab => LoadGameUIPrefab();

        private GameObject LoadGameUIPrefab()
        {
            if (_gameUI == null) _gameUI = 
                    Resources.Load<GameObject>(string.Concat(ResourcePath.GameUIPrefabFolder, _gameUIPrefabName));
            return _gameUI;
        }
    }
}