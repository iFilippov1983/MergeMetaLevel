using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "GameData/Cells/CellPropertiesExtended", fileName = "Cell_n_event")]
    public class CellPropertiesExtended : ScriptableObject
    {
        [SerializeField] private List<Properties> _cellProperties;



        [Serializable]
        private class Properties
        { 
            public string Name;
        }
    }
}