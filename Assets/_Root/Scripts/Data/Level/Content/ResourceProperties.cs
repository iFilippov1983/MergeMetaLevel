using System;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "GameData/Cells/ResourceProperties", fileName = "Resource_type")]
    [Serializable]
    internal sealed class ResourceProperties : ContentProperties
    {
        [SerializeField] private ResouceType _resouceType;
        [SerializeField] private int _resourceAmount;
        [SerializeField] private ParticleSystem _pickupEffect;

        public ResouceType ResouceType => _resouceType;
        public int Amount => _resourceAmount;
        public ParticleSystem PickupEffect => _pickupEffect;

        public override ContentType GetContentType() => ContentType.Resource;
    }
}
