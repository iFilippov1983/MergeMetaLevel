using Configs.Meta;
using Configs.Tutorial;
using Data;
using UnityEngine;

namespace Configs
{
    public class StaticData : ScriptableObject
    {
        public MergeConfig Merge;
        public ShopConfig Shop;
        public DebugConfig Debug;
        public TutorialConfig Tutorial;
        public ImagesMap Images;
        public ResourcesConfig ResourcesConfig;

        public void SetCtx(CoreRoot root)
        {
            // Quests.SetCtx();
            Merge.SetCtx();
            Debug.SetCtx(root);
        }
    }
}