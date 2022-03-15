using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using UnityEngine;

namespace Level
{
    internal class MetaLevel
    {
        private LevelData _levelData;
        private CellView[] _cells;
        private CellProperties[] _cellsPropeties;
        private Dictionary<int, CellEntity> _cellsEntities;

        public Dictionary<int, CellEntity> CellEntities => _cellsEntities;

        public MetaLevel(LevelData levelData)
        {
            _levelData = levelData;
            _cells = _levelData.CellsViews;
            _cellsPropeties = _levelData.CellsPropeties;
            _cellsEntities = new Dictionary<int, CellEntity>();
            FillLevel();
        }

        public List<CellEntity> GetRouteCellsFrom(int cellID)
        {
            var routeCells = new List<CellEntity>();
            for (int i = cellID; i < _cellsEntities.Count; i++)
            {
                var cellEntity = _cellsEntities[i];
                routeCells.Add(cellEntity);
                if (cellEntity.Propeties.Status.Equals(CellStatus.ToVisit)) break;
            }
            return routeCells;
        }

        private void FillLevel()
        {
            if (_cells.Length != _cellsPropeties.Length) return;
            for (int index = 0; index < _cells.Length; index++)
            {
                SetViewNumber(_cells[index], index, _cells.Length - 1);
                var entity = new CellEntity(index, _cells[index], _cellsPropeties[index]);
                _cellsEntities.Add(index, entity);
            }
        }

        private void SetViewNumber(CellView view, int number, int FinishCounter)
        {
            if (number == 0) view.TextMeshPro.text = "Start";
            else if (number == FinishCounter) view.TextMeshPro.text = "Finish";
            else view.TextMeshPro.text = number.ToString();
        }
    }
}
