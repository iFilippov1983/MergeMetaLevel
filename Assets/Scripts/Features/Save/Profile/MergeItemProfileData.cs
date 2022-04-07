using System;
using Entitas;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class MergeItemProfileData 
    {
        [SerializeReference]
        [HorizontalGroup(), HideLabel]
        public MergeItemConfig config;
        
        [HorizontalGroup(), HideLabel]
        public int x, y;

        [HorizontalGroup(), HideLabel]
        public int lockCount;
        
        // Dynamic Data . TODO : remove to dynamic data

        [HorizontalGroup(), HideLabel]
        [HideInInspector, NonSerialized]
        public int unlockCount;

        [HideInInspector, NonSerialized]
        [HorizontalGroup(), HideLabel]
        public int lifeTargetCount;
        
        public bool locked => lockCount > unlockCount;
        public int RestLocks => lockCount - unlockCount;

        public MergeItemProfileData(MergeItemConfig config, int x, int y, int lockCount)
        {
            this.config = config;
            this.x = x;
            this.y = y;
            this.lockCount = lockCount;
            this.unlockCount = 0;
        }

        public MergeItemProfileData Clone()
        {
            return new MergeItemProfileData(config, x, y, lockCount);
        }
    }
}