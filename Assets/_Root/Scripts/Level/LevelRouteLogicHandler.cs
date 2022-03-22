using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Level
{
    internal class LevelRouteLogicHandler
    {
        private CellProperties[] _fullLevelRoute;
        private CellProperties[] _cellsToVisitProperties;

        public LevelRouteLogicHandler(CellProperties[] cellsToVisitProperties)
        {
            _cellsToVisitProperties = cellsToVisitProperties;
            _fullLevelRoute = MakeFullLevelRoute(_cellsToVisitProperties);
        }

        public int GetRouteCountFrom(int cellId) => GetRouteIDsFrom(cellId).Count;

        public CellProperties GetCellToVisitPropertyWhithId(int cellId)
        {
            foreach (CellProperties cellProperty in _cellsToVisitProperties)
                if (cellProperty.Id.Equals(cellId)) return cellProperty;

            return null;
        }

        public CellProperties GetCellPropertyWhithId(int cellId)
        {
            foreach (CellProperties cellProperty in _fullLevelRoute)
                if (cellProperty.Id.Equals(cellId)) return cellProperty;

            return null;
        }

        public List<int> GetRouteIDsFrom(int cellId)
        {
            var routeCellsIDs = new List<int>();
            for (int i = cellId; i < _fullLevelRoute.Length; i++)
            {
                int id = _fullLevelRoute[i].Id;
                routeCellsIDs.Add(id);
                if (_fullLevelRoute[i].Status.Equals(CellStatus.ToVisit)) break;
            }
            return routeCellsIDs;
        }

        private CellProperties[] MakeFullLevelRoute(CellProperties[] cellsToVisitProperties)
        {
            var fullLevelRouteList = new List<CellProperties>();
            var lastCellId = cellsToVisitProperties[cellsToVisitProperties.Length - 1].Id;
            bool dontSkipEmpty = true;

            for (int index = 0; index < lastCellId + 1; index++)
            {
                foreach (CellProperties cellProperties in cellsToVisitProperties)
                {
                    if (cellProperties.Id.Equals(index))
                    {
                        fullLevelRouteList.Add(cellProperties);
                        dontSkipEmpty = false;
                        break;
                    }
                    else dontSkipEmpty = true; 
                }
                if (dontSkipEmpty)
                {
                    var emptyProperties = (CellProperties)ScriptableObject.CreateInstance(nameof(CellProperties));
                    emptyProperties.Id = index;
                    fullLevelRouteList.Add(emptyProperties);
                }
            }
            return fullLevelRouteList.ToArray();
        }
    }
}
