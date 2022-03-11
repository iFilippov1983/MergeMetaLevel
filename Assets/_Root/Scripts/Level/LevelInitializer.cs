using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Tool;

namespace Level
{
    internal class LevelInitializer
    {
        private LevelData _levelData;
        private CellView[] _cells;
        private CellProperties[] _cellsPropeties;
        private CellsEntity _cellsRepository;

        public CellsEntity CellsRepository => _cellsRepository;

        public LevelInitializer(LevelData levelData)
        {
            _levelData = levelData;
            _cells = _levelData.CellsViews;
            _cellsPropeties = _levelData.CellsPropeties;
            
            InitLevel();
            _cellsRepository = new CellsEntity(_cellsPropeties);
        }

        public void InitLevel()
        {
            if (_cells.Length != _cellsPropeties.Length) return;
            for (int index = 0; index < _cells.Length; index++)
            {
                _cellsPropeties[index].CellView = _cells[index];
                _cellsPropeties[index].Number = index;
                SetViewNumber(_cells[index], index, _cells.Length);
                //_cells[index].TextMeshPro.text = index.ToString();
            }
        }

        public void Dispose()
        {
            _cellsRepository.Dispose();
        }

        private void SetViewNumber(CellView view, int number, int FinishCounter)
        {
            if (number == 0) view.TextMeshPro.text = "Start";
            else if (number == FinishCounter - 1) view.TextMeshPro.text = "Finish";
            else view.TextMeshPro.text = number.ToString();
        }
    }


}
