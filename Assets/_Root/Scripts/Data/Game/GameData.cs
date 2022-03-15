using System.IO;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "GameData/GameData", fileName = "GameData")]
    public class GameData : ScriptableObject
    {
        private const string GameDataFolderPath = "GameData/";

        [SerializeField] private string _levelDataPath;
        [SerializeField] private string _playerDataPath;
        [SerializeField] private string _uiDataPath;

        private LevelData _levelData;
        private PlayerData _playerData;
        private UIData _uiData;

        public LevelData LevelData
        {
            get 
            {
                if (_levelData == null) _levelData =
                             LoadPath<LevelData>(string.Concat(GameDataFolderPath, _levelDataPath));
                return _levelData;
            }
        }

        public PlayerData PlayerData
        {
            get
            {
                if (_playerData == null) _playerData =
                             LoadPath<PlayerData>(string.Concat(GameDataFolderPath, _playerDataPath));
                return _playerData;
            }
        }

        public UIData UIData
        {
            get 
            { 
                if(_uiData == null) _uiData =
                        LoadPath<UIData>(string.Concat(GameDataFolderPath, _uiDataPath));
                return _uiData;
            }
        }


        private T LoadPath<T>(string path) where T : Object =>
            Resources.Load<T>(Path.ChangeExtension(path, null));
    }
}