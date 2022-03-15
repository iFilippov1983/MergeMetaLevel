using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Tool
{
    internal class ResourcesLoader
    {
        public static Sprite LoadSprite(string path) =>
            Resources.Load<Sprite>(path);

        public static GameObject LoadPrefab(string path) =>
            Resources.Load<GameObject>(path);
    }
}
