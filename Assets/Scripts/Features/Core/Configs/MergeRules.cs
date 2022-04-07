using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Data
{
    public class MergeRules : ScriptableObject
    {
        public enum MergeGenerateRuleType
        {
            Next,
            Same
        }

        [Serializable]
        public class GenerateRule
        {
            public int MergeCount;
            public GenerateCount GenerateCount;
        }

        [Serializable]
        public class GenerateCount
        {
            public List<MergeGenerateRuleType> GeneratedTypes = new List<MergeGenerateRuleType>();
        }

        [TableList]
        public List<GenerateRule> GenerateRules;
        private static GenerateCount one = new GenerateCount() {GeneratedTypes = new List<MergeGenerateRuleType>() {MergeGenerateRuleType.Next} };

        public GenerateCount GenerateRulesGet(int mergeCount)
        {
            var itemsToGenerate = GenerateRules.Find(v => v.MergeCount == mergeCount);
            return itemsToGenerate != null ? itemsToGenerate.GenerateCount : one;
        } 
    }
}