using Data;
using UnityEngine;

namespace Core
{
    public class LevelConfigService
    {
        private readonly MergeConfig _config;

        public LevelConfigService(MergeConfig config)
        {
            _config = config;
        }
        
        public LevelConfig Load(int level)
        {
            return _config.Levels[level];
                
            // var textAsset = Resources.Load<TextAsset>($"Levels/level_{level}");
            // var levelConfig = JsonUtility.FromJson<LevelConfig_Old>(textAsset.text);
            // return levelConfig;
        }
    }
}