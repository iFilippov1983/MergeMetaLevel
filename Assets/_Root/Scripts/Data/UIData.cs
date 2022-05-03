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
        [SerializeField] private string _goldParticlesPrefabName;
        
        private GameObject _gameUI;
        private GameObject _cameras;
        private GameObject _goldParticles;

        public GameObject GameUIPrefab => LoadGameUIPrefab();
        public GameObject CamerasPrefab => LoadCamerasPrefab();
        public GameObject GoldParticlesPrefab => LoadGoldParticlesPrefab();

        private GameObject LoadGoldParticlesPrefab()
        {
            if (_goldParticles == null) _goldParticles =
                    Resources.Load<GameObject>(string.Concat(ResourcePath.VfxPrefabsFolder, _goldParticlesPrefabName));
            return _goldParticles;
        }

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