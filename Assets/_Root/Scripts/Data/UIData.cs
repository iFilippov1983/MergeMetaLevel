using UnityEditor;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "GameData/UIData", fileName = "UIData")]
    public class UIData : ScriptableObject
    {
        [SerializeField] private Transform _uiContainer;

        public Transform UIContainer => _uiContainer;
    }
}