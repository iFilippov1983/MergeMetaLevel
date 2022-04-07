using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Utils;

namespace Data
{
    [Serializable]
    public class MergeDynamicData 
    {
        public LevelConfig Level;
        public int PlayerMoves;
        
        [TableList, ShowInInspector]
        public Dictionary<MergeItemConfig, int> MergeItemsCount = new Dictionary<MergeItemConfig, int>();
        
        public bool GameStart;
        public bool GameWin;

        public InputDynamicData Input = new InputDynamicData();
        
        private Dictionary<(int, int), ChipsEntity> _entityByPos = new Dictionary<(int, int), ChipsEntity>();
        
        public class InputDynamicData
        {
            public bool InputLocked;
            public bool MouseDown;
            public bool MousePressed;
            public bool MouseUp;
        };

        public void Clear()
        {
            _entityByPos.Clear();
        }

        public int ChipsCount(MergeItemConfig goalConfig)
        {
            var count = 0;
            foreach (var value in _entityByPos.Values)
            {
                if (value.chipInfo.config == goalConfig)
                    count++;
            }

            return count;
        }
        
        public List<ChipsEntity> FindAll(Predicate<ChipsEntity> match)
        {
            var res = new List<ChipsEntity>();
            foreach (var kv in _entityByPos)
            {
                if(match(kv.Value))
                    res.Add(kv.Value);
            }

            return res;
        }

        public int Key(MergeItemProfileData itemData) => Key(itemData.x, itemData.y );
        public int Key(int x, int y) => y * 100 + x;

        public ChipsEntity GetItem(int x, int y) 
            => _entityByPos.SaveGet((x, y));

        public void AddPos(ChipsEntity e, int x, int y) 
            => _entityByPos.Add((x, y), e);

        public void RemovePos(ChipsEntity e, int x, int y) 
            => _entityByPos.Remove((x, y));

        public void ReplacePos(ChipsEntity e, int oldX, int oldY, int x, int y)
        {
            RemovePos(e, oldX, oldY);
            AddPos(e, x, y);
        }
    }
}