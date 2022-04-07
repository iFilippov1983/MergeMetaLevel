using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Configs.Meta
{
    [Serializable]
    public class ResourceInfo
    {
        [PreviewField]
        public Sprite Icon;
        public string Name;
        [Multiline(3)]
        public string Description;
    }
}