using System;
using UnityEngine;

namespace Data
{
    [Serializable]
    public struct PlayerStats
    {
        [SerializeField] private int _power;
        [SerializeField] private int _powerToHealthMultiplyer;

        public int Power
        {
            get => _power;
            set { _power = value; }
        }

        public int Health => _power * _powerToHealthMultiplyer;

        public int Gold;
        public int Gems;
        public int DiceRolls;
        public bool LastFightWinner;
        [Space]
        public int CurrentCellID;
    }
}
