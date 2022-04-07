using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Level
{
    internal class LevelViewHandler
    {
        private LevelData _levelData;
        private CellView[] _cells;
        private Dictionary<int, CellView> _cellsViewDictionary;

        public Dictionary<int, CellView> CellsViewDictionary => _cellsViewDictionary;

        public LevelViewHandler(LevelData levelData)
        {
            _levelData = levelData;
            _cells = _levelData.CellsViews;
            _cellsViewDictionary = new Dictionary<int, CellView>();
            for (int index = 0; index < _cells.Length; index++)
            {
                _cells[index].CellBodyMeshRenderer.material = _cells[index].ActualMaterial;
                _cellsViewDictionary.Add(index, _cells[index]);
            }
        }

        public Vector3 GetCellPositionWithId(int id) => _cellsViewDictionary[id].transform.position;

        public CellView GetCellViewWithId(int id, bool makeEnemyPointsActive = false)
        {
            var cell = _cellsViewDictionary[id];
            cell.EnemySpawnPoint.gameObject.SetActive(makeEnemyPointsActive);
            return cell;
        }
    }
}
