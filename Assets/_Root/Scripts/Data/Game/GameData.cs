using System.IO;
using Tool;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "GameData/GameData", fileName = "GameData")]
    public class GameData : ScriptableObject
    {
        [SerializeField] private string _levelDataPath;
        [SerializeField] private string _playerDataPath;
        [SerializeField] private string _enemiesDataPath;
        [SerializeField] private string _uiDataPath;
        [SerializeField] private string _progressDataPath;

        private LevelData _levelData;
        private PlayerData _playerData;
        private EnemiesData _enemiesData; 
        private UIData _uiData;
        private ProgressData _progressData;

        public LevelData LevelData
        {
            get 
            {
                if (_levelData == null) _levelData =
                             LoadPath<LevelData>(string.Concat(ResourcePath.GameDataFolder, _levelDataPath));
                return _levelData;
            }
        }

        public PlayerData PlayerData
        {
            get
            {
                if (_playerData == null) _playerData =
                             LoadPath<PlayerData>(string.Concat(ResourcePath.GameDataFolder, _playerDataPath));
                return _playerData;
            }
        }

        public EnemiesData EnemiesData
        {
            get
            {
                if (_enemiesData == null) _enemiesData =
                             LoadPath<EnemiesData>(string.Concat(ResourcePath.GameDataFolder, _enemiesDataPath));
                return _enemiesData;
            }
        }

        public UIData UIData
        {
            get 
            { 
                if(_uiData == null) _uiData =
                        LoadPath<UIData>(string.Concat(ResourcePath.GameDataFolder, _uiDataPath));
                return _uiData;
            }
        }

        public ProgressData ProgressData
        {
            get
            {
                if (_progressData == null) _progressData =
                             LoadPath<ProgressData>(string.Concat(ResourcePath.GameDataFolder, _progressDataPath));
                return _progressData;
            }
        }

        private T LoadPath<T>(string path) where T : Object =>
            Resources.Load<T>(Path.ChangeExtension(path, null));
    }
}