using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "GameData/Cells/ResourceProperties", fileName = "Resource_type_Properties")]
    internal class ResourceProperties : ScriptableObject, ICellContentProperties
    {
        [SerializeField] private ResouceType _resouceType;
        [SerializeField] private int _resourceAmount;

        public ResouceType ResouceType => _resouceType;
        public int Amount => _resourceAmount;
        public ContentType GetContentType() => ContentType.Resource;
    }
}
