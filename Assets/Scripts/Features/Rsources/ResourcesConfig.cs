using System.Collections.Generic;
using Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Configs.Meta
{
    public class ResourcesConfig : SerializedScriptableObject
    {
        [TableList]
        public Dictionary<ResourceType, ResourceInfo> Resources = new Dictionary<ResourceType, ResourceInfo>();

        public Sprite GetImage(ResourceType type)
        {
            if (!Resources.TryGetValue(type, out ResourceInfo info))
                return null;
            return info.Icon;
        }
    }
}