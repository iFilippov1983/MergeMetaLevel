using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public CellView GetCellViewWithID(int id) => _cellsViewDictionary[id];
        public Vector3 GetCellPositionWithID(int id) => _cellsViewDictionary[id].transform.position;
        public LevelViewHandler(LevelData levelData)
        {
            _levelData = levelData;
            _cells = _levelData.CellsViews;
            _cellsViewDictionary = new Dictionary<int, CellView>();
            Initialize();
        }

        private void Initialize()
        {
            for (int index = 0; index < _cells.Length; index++)
            {
                SetViewNumber(_cells[index], index, _cells.Length - 1);
                _cellsViewDictionary.Add(index, _cells[index]);
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
