using System;
using Tool;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "GameData/Cells/ResourceProperties", fileName = "Resource_type")]
    [Serializable]
    internal sealed class ResourceProperties : ContentProperties
    {
        [SerializeField] private ResouceType _resouceType;
        [SerializeField] private int _resourceAmount;
        [SerializeField] private string _pickupEffectPrefabName;

        private GameObject _pickupEffectPrefab;

        public ResouceType ResouceType => _resouceType;
        public int Amount => _resourceAmount;
        public GameObject PickupEffectPrefab
        {
            get
            {
                if (_pickupEffectPrefab == null) _pickupEffectPrefab =
                        Resources.Load<GameObject>(string.Concat(ResourcePath.VfxPrefabsFolder, _pickupEffectPrefabName));
                return _pickupEffectPrefab;
            }
        }

        public override ContentType GetContentType() => ContentType.Resource;

    }
}
