using Level;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "GameData/LevelData", fileName = "LevelData")]
    public class LevelData : ScriptableObject
    {
        [SerializeField] private GameObject _level;
        [SerializeField] private CellProperties[] _cellsPropeties;

        public CellView[] CellsViews =>  GetCellsViews();
        public CellProperties[] CellsPropeties => _cellsPropeties;

        private CellView[] GetCellsViews()
        {
            if (_level == null)
            {
                var viewObject = FindObjectOfType<LevelView>();
                _level = viewObject.gameObject;
            }
            return _level.GetComponentsInChildren<CellView>();
        }
    }

    public enum CellStatus { ToSkip, ToVisit }
    public enum ContentType { None, Resource, Enemy }
    public enum ResouceType { Gold, ExtraRoll, Gem }
    public enum EnemyType { Barbarian, Rogue, Witch }
}