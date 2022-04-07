using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace Data
{
    public class LevelConfig : SerializedScriptableObject
    {
        public int Width = 8;
        public int Height = 8;
        public int FakeWidth = 0;
        public int FakeHeight = 0;
        public int MaxWidth => Mathf.Max(Width, FakeWidth);
        public int MaxHeight => Mathf.Max(Height, FakeHeight);
        
        [TableList]
        public List<V2> Holes = new List<V2>();
        
        [TableList]
        public List<MergeItemProfileData> Items;

        public MergeGoal Goal;
        public int Moves = 15;

        [Serializable]
        public class MergeGoal
        {
            public MergeItemConfig Config;
            public int Count = 1;
        }
    }
}