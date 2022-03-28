using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Data
{
    [Serializable]
    internal sealed class ContentResource : ContentData
    {
        [SerializeField] private ResouceType _resouceType;
        [SerializeField] private int _resourceAmount;

        public ResouceType ResouceType => _resouceType;
        public int Amount => _resourceAmount;

        public override ContentType GetContentType() => ContentType.Resource;
    }
}
