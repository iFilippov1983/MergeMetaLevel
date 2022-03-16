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
        [SerializeField] private List<ScriptableObject> _cellContent;
        [SerializeField] private Sprite _sprite;

        public CellStatus Status => _status;
        public List<ScriptableObject> Content => _cellContent;
        public Sprite Sprite => _sprite;
    }

}

