using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Components;
using Core;

namespace Data
{
    public class MergeConfig : ScriptableObject
    {
        public List<LevelConfig> Levels;

        public MergeVisualConfig VisualConfig;
        public List<MergeSequenceConfig> MergeSequences;
        public MergePrefabs Prefabs;

        public MergeRules Rules;
        public MergeSequenceConfig CoinsSequence;
        // public GameObject ActiveHighlight <<<< ; // Подсветка под активный декор
                
        // Dynamic
        private Dictionary<string, MergeItemConfig> _cache = new Dictionary<string, MergeItemConfig>();
        
        public void SetCtx()
        {
            FillCache();
        }
        
        public MergeItemConfig Get(string name)
        {
            if (!_cache.TryGetValue(name, out MergeItemConfig result))
                throw new Exception($"MergeableConfig not found {name}");
        
            return result;
        }
        
        [Button]
        private void FillCache()
        {
            _cache.Clear();
            foreach (var sequence in MergeSequences)
            foreach (var item in sequence.Items)
                _cache.Add(item.name, item);
        }
    }
}