using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Level;

namespace Data
{
    [CreateAssetMenu(menuName = "GameData/Cells/CellProperties", fileName = "CellProperties")]
    public class CellProperties : ScriptableObject
    {
        public int Id;
        [SerializeField] private CellStatus _status;
        [SerializeField] private ContentProperties _contentProperties;

        //[SerializeField] private List<ContentProperties>

        public CellStatus Status => _status;
        internal ContentProperties ContentProperties => _contentProperties;
    }
}

