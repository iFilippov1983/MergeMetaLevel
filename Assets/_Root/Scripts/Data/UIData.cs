using UnityEngine;
using Tool;
using System;

namespace Data
{
    [CreateAssetMenu(menuName = "GameData/UIData", fileName = "UIData")]
    public class UIData : ScriptableObject
    {
        [SerializeField] private string _gameUIPrefabName;
        [SerializeField] private string _camerasPrefabName;
        
        private GameObject _gameUI;
        private GameObject _cameras;

        public GameObject GameUIPrefab => LoadGameUIPrefab();
        public GameObject CamerasPrefab => LoadCamerasPrefab();

        private GameObject LoadCamerasPrefab()
        {
            if (_cameras == null) _cameras =
                     Resources.Load<GameObject>(string.Concat(ResourcePath.PrefabsFolder, _camerasPrefabName));
            return _cameras;
        }

        private GameObject LoadGameUIPrefab()
        {
            if (_gameUI == null) _gameUI = 
                    Resources.Load<GameObject>(string.Concat(ResourcePath.PrefabsFolder, _gameUIPrefabName));
            return _gameUI;
        }
    }
}