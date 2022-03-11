using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Level;

namespace Data
{
    [CreateAssetMenu(menuName = "GameData/Cells/CellProperties", fileName = "CellProperties")]
    public class CellProperties : ScriptableObject
    {
        [SerializeField] private CellStatus _status;
        [SerializeField] private List<ScriptableObject> _cellContent;
        private int _cellNumber;
        private CellView _cellView;//!!!!
        

        public CellStatus Status => _status;
        public List<ScriptableObject> Content => _cellContent;
        public int Number
        {
            get => _cellNumber;
            set
            {
                if (_cellNumber == 0) _cellNumber = value;
            }
        }
        public CellView CellView
        {
            get => _cellView;
            set
            {
                if (_cellView == null) _cellView = value;
            }
        }
    }

}

