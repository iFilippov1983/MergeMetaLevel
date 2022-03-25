using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "GameData/Cells/CellProperties", fileName = "CellProperties")]
    public class CellProperties : ScriptableObject
    {
        public int Id;
        [SerializeField] private CellStatus _status;
        [SerializeField] private ContentProperties _contentProperties;

        public CellStatus Status => _status;
        internal ContentProperties ContentProperties => _contentProperties;
    }
}

