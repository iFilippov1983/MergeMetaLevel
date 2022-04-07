using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Configs
{
    public class ImagesMap : SerializedScriptableObject
    {
        public Dictionary<string, Sprite> ByName = new Dictionary<string, Sprite>();
    }
}