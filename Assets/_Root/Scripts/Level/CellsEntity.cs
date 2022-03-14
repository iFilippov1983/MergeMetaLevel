﻿using Data;
using UnityEngine;

namespace Level
{
    internal class CellsEntity
    {
        private int _number;
        private CellView _cellView;
        CellProperties _propeties;
        public CellsEntity(int number, CellView cellView, CellProperties properties)
        {
            _number = number;
            _cellView = cellView;
            _propeties = properties;
        }

        public int Number => _number;
        public CellView CellView => _cellView; 
        public CellProperties Propeties => _propeties;
        public Vector3 Position => _cellView.transform.position;
    }
}
