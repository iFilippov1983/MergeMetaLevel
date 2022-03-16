using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Level
{
    internal class LevelLogicHandler
    {
        private CellProperties[] _cellsProperties;

        public CellProperties[] CellProperties => _cellsProperties;

        public LevelLogicHandler(CellProperties[] cellsProperties)
        {
            _cellsProperties = cellsProperties;
            SetIDs();
        }

        public List<CellProperties> GetRouteCellsPropertiesFrom(int cellID)
        {
            var routeCellsProperties = new List<CellProperties>();
            for (int i = cellID; i < _cellsProperties.Length; i++)
            {
                var cellProperties = _cellsProperties[i];
                routeCellsProperties.Add(cellProperties);
                if (cellProperties.Status.Equals(CellStatus.ToVisit)) break;
            }
            return routeCellsProperties;
        }

        private void SetIDs()
        {
            for (int index = 0; index < _cellsProperties.Length; index++)
            {
                _cellsProperties[index].Id = index;
            }
        }
    }
}
