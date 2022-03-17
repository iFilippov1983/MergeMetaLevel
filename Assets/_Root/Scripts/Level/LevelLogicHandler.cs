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
        private CellProperties[] _fullLevelRoute;
        private CellProperties[] _cellsToVisitProperties;

        private CellProperties[] _cellsProperties;

        //public CellProperties[] CellProperties => _cellsProperties;


        public LevelLogicHandler(CellProperties[] cellsToVisitProperties)   //(CellProperties[] cellsProperties)
        {
            //_cellsProperties = cellsProperties;
            //SetIDs();
            _cellsToVisitProperties = cellsToVisitProperties;
            MakeFullLevelRoute();
        }

        public int GetRouteCountFrom(int cellId) => GetRouteIDsFrom(cellId).Count;

        public CellProperties GetCellPropertyWhithId(int cellId)
        {
            foreach (CellProperties cellProperty in _cellsToVisitProperties)
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

        private void MakeFullLevelRoute()
        {
            var fullLevelRouteList = new List<CellProperties>();
            var lastCellId = _cellsToVisitProperties[_cellsToVisitProperties.Length - 1].Id;
            bool dontSkipEmpty = true;

            for (int index = 0; index < lastCellId + 1; index++)
            {
                foreach (CellProperties cellProperties in _cellsToVisitProperties)
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
            _fullLevelRoute = fullLevelRouteList.ToArray();
        }

        //public List<CellProperties> GetRouteCellsPropertiesFrom(int cellId)
        //{
        //    var routeCellsProperties = new List<CellProperties>();
        //    for (int i = cellId; i < _cellsProperties.Length; i++)
        //    {
        //        var cellProperties = _cellsProperties[i];
        //        routeCellsProperties.Add(cellProperties);
        //        if (cellProperties.Status.Equals(CellStatus.ToVisit)) break;
        //    }
        //    return routeCellsProperties;
        //}

        //private void SetIDs()
        //{
        //    for (int index = 0; index < _cellsProperties.Length; index++)
        //    {
        //        _cellsProperties[index].Id = index;
        //    }
        //}
    }
}
