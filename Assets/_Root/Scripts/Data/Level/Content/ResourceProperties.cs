using System;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "GameData/Cells/ResourceProperties", fileName = "Resource_type_Properties")]
    [Serializable]
    internal sealed class ResourceProperties : ContentProperties
    {
        [SerializeField] private ResouceType _resouceType;
        [SerializeField] private int _resourceAmount;

        public ResouceType ResouceType => _resouceType;
        public int Amount => _resourceAmount;

        public override ContentType GetContentType() => ContentType.Resource;
    }
}
