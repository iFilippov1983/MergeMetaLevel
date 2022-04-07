using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class DecoreInfo
    {
        public List<GameObject> Skins = new List<GameObject>();
        public List<Sprite> SkinPreview = new List<Sprite>();
    }
}