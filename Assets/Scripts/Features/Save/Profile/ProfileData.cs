using System;
using System.Collections.Generic;
using Configs.Quests;
using Features._Events;
using Newtonsoft.Json;
using Sirenix.OdinInspector;

namespace Data
{
    [Serializable]
    public class ProfileData
    {
        [TableList]
        public Dictionary<ResourceType, int> Resources = new Dictionary<ResourceType, int>();
        public int TutorialIndex = 0;
        public DateTime HeartsChangedTime;

        private DataEvents _events;

        public void SetCtx(RootEvents events)
            => _events = events.Data;

        [JsonIgnore]
        public int LevelIndex
        {
            get => Get(ResourceType.Level);
            set => Set(ResourceType.Level, value);
        }


        [JsonIgnore]
        public int Coins
        {
            get => Get(ResourceType.Coins);
            set => Set(ResourceType.Coins, value);
        }
        
        [JsonIgnore]
        public int Hearts
        {
            get => Get(ResourceType.Hearts);
            set => Set(ResourceType.Hearts, value);
        }
        
        [JsonIgnore]
        public int Diamonds
        {
            get => Get(ResourceType.Diamonds);
            set => Set(ResourceType.Diamonds, value);
        }

        [JsonIgnore]
        public int Xp
        {
            get => Get(ResourceType.Xp);
            set => Set(ResourceType.Xp, value);
        }

        public void Add(List<Resource> rewards)
        {
            foreach (var res in rewards)
                Add(res.Type, res.Count);
        }
        
        public void Add(ResourceType type, int count)
        {
            if (!Resources.TryGetValue(type, out int res))
                Set(type, count);
            else
                Set(type, res + count);
        }
        
        private int Get(ResourceType type)
        {
            if (!Resources.TryGetValue(type, out int res))
                Resources[type] = 0;

            return res;
        }
        
        private void Set(ResourceType type, int value)
        {
            var was = Resources[type];
            Resources[type] = value;
            _events.OnResourceChanged?.Invoke(type, value, was);
        }

        public void Patch()
        {
            if (Resources.Count == 0)
                CreateInitialInventory();
        }

        private void CreateInitialInventory()
        {
            Resources = new Dictionary<ResourceType, int>()
            {
                [ResourceType.Level] = 0,
                [ResourceType.Hearts] = 0,
                [ResourceType.Coins] = 1000,
            };
        }
        
    }
}